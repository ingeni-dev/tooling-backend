using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PTCwebApi.Interfaces;
using PTCwebApi.Models.ProfilesModels;
using PTCwebApi.Models.PTCModels;
using PTCwebApi.Models.PTCModels.MethodModels;
using PTCwebApi.Models.PTCModels.MethodModels.FindTools;
using PTCwebApi.Models.PTCModels.MethodModels.MoveLoc;

namespace PTCwebApi.Methods.PTCMethods
{
    public class PTCMethods
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IMapper _mapper;
        public PTCMethods(IMapper mapper, IJwtGenerator jwtGenerator)
        {
            _jwtGenerator = jwtGenerator;
            _mapper = mapper;
        }
        //*Find  Lastest location of Tool
        public async Task<ReturnDataTool> FindLocOfTooling(RequestAllModelsPTC model)
        {
            string _returnFlag = "0";
            string _returnText = "ผ่าน";
            GetLoc _dataLoc = new GetLoc();

            var querySN = $"SELECT DIECUT_SN, DIECUT_TYPE FROM KPDBA.DIECUT_SN WHERE DIECUT_ID ='{model.ptcID}' OR DIECUT_SN ='{model.ptcID}'";
            var resultSN = await new DataContext().GetResultDapperAsyncDynamic(DataBaseHostEnum.KPR, querySN);
            model.ptcID = (resultSN as List<dynamic>)[0].DIECUT_SN;
            var ptcType = (resultSN as List<dynamic>)[0].DIECUT_TYPE;
            decimal toolValid = (resultSN as List<dynamic>).Count;

            if (toolValid == 1)
            {
                //* check location of the tool.
                var query = $"SELECT SD.LOC_ID, SD.COMP_ID, LOC.LOC_DETAIL, SD.QTY FROM(SELECT SD.WAREHOUSE_ID, SD.LOC_ID, SD.COMP_ID, SUM (SD.QTY) QTY FROM KPDBA.PTC_STOCK_DETAIL SD WHERE SD.WAREHOUSE_ID = '{model.warehouseID}' AND SD.PTC_ID = '{model.ptcID}' GROUP BY SD.WAREHOUSE_ID, SD.LOC_ID, SD.COMP_ID HAVING SUM (SD.QTY) > 0) SD JOIN(SELECT WAREHOUSE_ID, LOC_ID, LOC_DETAIL FROM KPDBA.LOCATION_PTC WHERE PTC_TYPE = '{ptcType}') LOC ON (SD.WAREHOUSE_ID = LOC.WAREHOUSE_ID AND SD.LOC_ID = LOC.LOC_ID)";
                var result = await new DataContext().GetResultDapperAsyncObject(DataBaseHostEnum.KPR, query);
                decimal count = (result as List<object>).Count;
                if (count == 0)
                {
                    _returnFlag = "1";
                    _returnText = "ไม่พบอุปกรณ์คงเหลือภายในคลัง";
                }
                else
                {
                    var results = _mapper.Map<IEnumerable<GetLoc>>(result);
                    _dataLoc = results.ElementAt(0);
                }
            }
            else
            {
                _returnFlag = "1";
                _returnText = "ไม่พบรหัส Tooling นี้ในฐานข้อมูล";
            }
            var returnResult = new ReturnDataTool
            {
                locID = _dataLoc.LOC_ID,
                locName = _dataLoc.LOC_DETAIL,
                flag = _returnFlag,
                text = _returnText
            };
            return returnResult;
        }
        //*Find suggess location of Tool
        public async Task<ReturnDataTool> FindSuggLocOfTooling(RequestAllModelsPTC model)
        {
            string _returnFlag = "1";
            string _returnText = "ไม่มีพื้นที่แนะนำ สำหรับจัดเก็บอุปกรณ์นี้";
            GetLoc _dataLoc = new GetLoc();

            //* check A tool is real in warehourse.
            var queryCheck = $"SELECT COUNT(1) AS COUN FROM KPDBA.DIECUT_SN WHERE DIECUT_SN ='{model.ptcID}'";
            var resultCheck = await new DataContext().GetResultDapperAsyncDynamic(DataBaseHostEnum.KPR, queryCheck);
            decimal toolValid = (resultCheck as List<dynamic>)[0].COUN;

            if (toolValid == 1)
            {
                //* check location of the tool.
                var query = $"SELECT SD.LOC_ID, LOC.LOC_DETAIL, SD.QTY FROM (SELECT SD.WAREHOUSE_ID, SD.LOC_ID, SUM (SD.QTY) QTY FROM KPDBA.PTC_STOCK_DETAIL SD WHERE SD.WAREHOUSE_ID = '{model.warehouseID}' AND SD.PTC_ID = '{model.ptcID}' GROUP BY SD.WAREHOUSE_ID, SD.LOC_ID HAVING SUM (SD.QTY) > 0) SD JOIN (SELECT WAREHOUSE_ID, LOC_ID, LOC_DETAIL FROM KPDBA.LOCATION_PTC WHERE RECEIVE_LOC_FLAG = 'T') LOC ON (SD.WAREHOUSE_ID = LOC.WAREHOUSE_ID AND SD.LOC_ID = LOC.LOC_ID)";
                var result = await new DataContext().GetResultDapperAsyncObject(DataBaseHostEnum.KPR, query);
                decimal count = (result as List<object>).Count;
                if (count == 0)
                {
                    _dataLoc.LOC_ID = "$R70";
                    _dataLoc.LOC_DETAIL = "พื้นที่รอจัดเก็บ";
                    _returnFlag = "0";
                    _returnText = "ผ่าน";
                }
                else
                {
                    //มี
                    var results = _mapper.Map<IEnumerable<GetLoc>>(result);
                    _dataLoc = results.ElementAt(0);
                }
            }
            else
            {
                _returnFlag = "1";
                _returnText = "ไม่พบรหัส Tooling นี้ในฐานข้อมูล";
            }
            var returnResult = new ReturnDataTool
            {
                locID = _dataLoc.LOC_ID,
                locName = _dataLoc.LOC_DETAIL,
                flag = _returnFlag,
                text = _returnText
            };
            return returnResult;
        }

        //* Move Tool
        public async Task<ReturnDataMoveLoc> MoveTooling(RequestAllModelsPTC model)
        {
            string _returnFlag = "0";
            string _returnText = "ผ่าน";
            string ptcTypes;
            string transfer_flag = "F";
            string transfer_loc_flag = "T";
            List<string> insertQuery = new List<string>();
            if (model.token != null)
            {
                UserProfile userProfile = _jwtGenerator.DecodeToken(model.token);
                if (userProfile != null)
                {
                    var querySN = $"SELECT DIECUT_SN, DIECUT_TYPE FROM KPDBA.DIECUT_SN WHERE DIECUT_ID ='{model.ptcID}' OR DIECUT_SN ='{model.ptcID}'";
                    var resultSN = await new DataContext().GetResultDapperAsyncDynamic(DataBaseHostEnum.KPR, querySN);
                    model.ptcID = (resultSN as List<dynamic>)[0].DIECUT_SN;
                    ptcTypes = (resultSN as List<dynamic>)[0].DIECUT_TYPE;
                    decimal toolReal = (resultSN as List<dynamic>).Count;
                    if (toolReal == 1)
                    {
                        var queryCheckLoc = $"SELECT COUNT(1) AS COUN FROM KPDBA.LOCATION_PTC WHERE LOC_ID = '{model.locID}' AND WAREHOUSE_ID = '{model.warehouseID}' AND PTC_TYPE = '{ptcTypes}' AND TRANSFER_LOC_FLAG = '{transfer_flag}'";
                        var resultCheckLoc = await new DataContext().GetResultDapperAsyncDynamic(DataBaseHostEnum.KPR, queryCheckLoc);
                        decimal LocValid = (resultCheckLoc as List<dynamic>)[0].COUN;

                        var queryCrossWH = $"SELECT WAREHOUSE_ID FROM KPDBA.LOCATION_PTC WHERE LOC_ID = '{model.locID}' AND PTC_TYPE = '{ptcTypes}' AND TRANSFER_LOC_FLAG = '{transfer_loc_flag}'";
                        var resultCrossWH = await new DataContext().GetResultDapperAsyncDynamic(DataBaseHostEnum.KPR, queryCrossWH);
                        var rCrossWH = resultCrossWH as List<dynamic>;
                        decimal crossWHVal = rCrossWH.Count;

                        if (LocValid != 0)
                        {
                            var query = $"SELECT SD.LOC_ID, SD.COMP_ID, LOC.LOC_DETAIL, SD.QTY FROM(SELECT SD.WAREHOUSE_ID, SD.LOC_ID, SD.COMP_ID, SUM (SD.QTY) QTY FROM KPDBA.PTC_STOCK_DETAIL SD WHERE SD.WAREHOUSE_ID = '{model.warehouseID}' AND SD.PTC_ID = '{model.ptcID}' GROUP BY SD.WAREHOUSE_ID, SD.LOC_ID, SD.COMP_ID HAVING SUM (SD.QTY) > 0) SD JOIN(SELECT WAREHOUSE_ID, LOC_ID, LOC_DETAIL FROM KPDBA.LOCATION_PTC WHERE PTC_TYPE = '{ptcTypes}') LOC ON (SD.WAREHOUSE_ID = LOC.WAREHOUSE_ID AND SD.LOC_ID = LOC.LOC_ID)";
                            var result = await new DataContext().GetResultDapperAsyncObject(DataBaseHostEnum.KPR, query);
                            decimal count = (result as List<object>).Count;
                            if (count == 0)
                            {
                                _returnFlag = "1";
                                _returnText = "ไม่พบอุปกรณ์คงเหลือภายในคลัง";
                            }
                            else
                            {
                                var results = _mapper.Map<IEnumerable<GetLoc>>(result);
                                var dataLoc = results.ElementAt(0);
                                var queryCompID = $"SELECT COMP_ID COMP FROM KPDBA.WAREHOUSE WHERE WAREHOUSE_ID ='{model.warehouseID}'";
                                var resultCompID = await new DataContext().GetResultDapperAsyncObject(DataBaseHostEnum.KPR, queryCompID);
                                var compID = (resultCompID as List<dynamic>)[0].COMP;
                                var tranSEQ = 1;
                                var QTY = "-1";
                                var S_STATUS = 'T';
                                var tranType = "5"; // โอนย้ายออก
                                var locID = dataLoc.LOC_ID; // old loc
                                string tran_id = await new StoreConnectionMethod(_mapper).PtcGetTranID(compID: compID, tranType: "4");

                                var tranDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));
                                var insertGetQuery = $"INSERT INTO KPDBA.PTC_STOCK_DETAIL (TRAN_ID, TRAN_SEQ, TRAN_TYPE, TRAN_DATE,PTC_ID, QTY, COMP_ID, WAREHOUSE_ID, LOC_ID, STATUS, CR_DATE, CR_ORG_ID, CR_USER_ID, PTC_TYPE) VALUES ('{tran_id}', TO_NUMBER('{tranSEQ}'), TO_NUMBER('{tranType}'), TO_DATE('{tranDate}', 'dd/mm/yyyy hh24:mi:ss'),'{model.ptcID}', TO_NUMBER('{QTY}'), TO_CHAR('{compID}'),'{model.warehouseID}','{locID}', TO_CHAR('{S_STATUS}'), SYSDATE, '{userProfile.org}', '{userProfile.userID}', TO_CHAR('{ptcTypes}'))";
                                // var resultInsert = await new DataContext().InsertResultDapperAsync(DataBaseHostEnum.KPR, insertGetQuery);
                                insertQuery.Add(insertGetQuery);
                                tranSEQ = 2;
                                QTY = "1";
                                tranType = "4"; // โอนย้ายเข้า
                                locID = model.locID; // newLoc

                                tranDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));
                                var insertOutQuery = $"INSERT INTO KPDBA.PTC_STOCK_DETAIL (TRAN_ID, TRAN_SEQ, TRAN_TYPE, TRAN_DATE,PTC_ID, QTY, COMP_ID, WAREHOUSE_ID, LOC_ID, STATUS, CR_DATE, CR_ORG_ID, CR_USER_ID, PTC_TYPE) VALUES ('{tran_id}', TO_NUMBER('{tranSEQ}'), TO_NUMBER('{tranType}'), TO_DATE('{tranDate}', 'dd/mm/yyyy hh24:mi:ss'),'{model.ptcID}', TO_NUMBER('{QTY}'), TO_CHAR('{compID}'),'{model.warehouseID}','{locID}', TO_CHAR('{S_STATUS}'), SYSDATE, '{userProfile.org}', '{userProfile.userID}', TO_CHAR('{ptcTypes}'))";
                                // var resultOutInsert = await new DataContext().InsertResultDapperAsync(DataBaseHostEnum.KPR, insertOutQuery);
                                insertQuery.Add(insertOutQuery);
                                var resultInsertAll = await new DataContext().ExecuteDapperMultiAsync(DataBaseHostEnum.KPR, insertQuery);
                            }
                        }
                        else if (crossWHVal != 0)
                        {
                            string WH = rCrossWH[0].WAREHOUSE_ID;
                            if (WH != model.warehouseID)
                            {
                                var query = $"SELECT SD.LOC_ID, SD.COMP_ID, LOC.LOC_DETAIL, SD.QTY FROM(SELECT SD.WAREHOUSE_ID, SD.LOC_ID, SD.COMP_ID, SUM (SD.QTY) QTY FROM KPDBA.PTC_STOCK_DETAIL SD WHERE SD.WAREHOUSE_ID = '{model.warehouseID}' AND SD.PTC_ID = '{model.ptcID}' GROUP BY SD.WAREHOUSE_ID, SD.LOC_ID, SD.COMP_ID HAVING SUM (SD.QTY) > 0) SD JOIN(SELECT WAREHOUSE_ID, LOC_ID, LOC_DETAIL FROM KPDBA.LOCATION_PTC WHERE PTC_TYPE = '{ptcTypes}') LOC ON (SD.WAREHOUSE_ID = LOC.WAREHOUSE_ID AND SD.LOC_ID = LOC.LOC_ID)";
                                var result = await new DataContext().GetResultDapperAsyncObject(DataBaseHostEnum.KPR, query);
                                decimal count = (result as List<object>).Count;
                                if (count == 0)
                                {
                                    _returnFlag = "1";
                                    _returnText = "ไม่พบอุปกรณ์คงเหลือภายในคลัง";
                                }
                                else
                                {
                                    var results = _mapper.Map<IEnumerable<GetLoc>>(result);
                                    var dataLoc = results.ElementAt(0);
                                    var queryCompID = $"SELECT COMP_ID COMP FROM KPDBA.WAREHOUSE WHERE WAREHOUSE_ID ='{model.warehouseID}'";
                                    var resultCompID = await new DataContext().GetResultDapperAsyncObject(DataBaseHostEnum.KPR, queryCompID);
                                    var compID = (resultCompID as List<dynamic>)[0].COMP;
                                    var tranSEQ = 1;
                                    var QTY = "-1";
                                    var S_STATUS = 'T';
                                    var tranType = "15"; // โอนย้ายออก
                                    var locID = dataLoc.LOC_ID; // old loc
                                    string tran_id = await new StoreConnectionMethod(_mapper).PtcGetTranID(compID: compID, tranType: "4");

                                    var tranDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));
                                    var insertGetQuery = $"INSERT INTO KPDBA.PTC_STOCK_DETAIL (TRAN_ID, TRAN_SEQ, TRAN_TYPE, TRAN_DATE,PTC_ID, QTY, COMP_ID, WAREHOUSE_ID, LOC_ID, STATUS, CR_DATE, CR_ORG_ID, CR_USER_ID, PTC_TYPE) VALUES ('{tran_id}', TO_NUMBER('{tranSEQ}'), TO_NUMBER('{tranType}'), TO_DATE('{tranDate}', 'dd/mm/yyyy hh24:mi:ss'),'{model.ptcID}', TO_NUMBER('{QTY}'), TO_CHAR('{compID}'),'{model.warehouseID}','{locID}', TO_CHAR('{S_STATUS}'), SYSDATE, '{userProfile.org}', '{userProfile.userID}', TO_CHAR('{ptcTypes}'))";
                                    // var resultInsert = await new DataContext().InsertResultDapperAsync(DataBaseHostEnum.KPR, insertGetQuery);
                                    insertQuery.Add(insertGetQuery);
                                    tranSEQ = 2;
                                    QTY = "1";
                                    tranType = "14"; // โอนย้ายเข้า
                                    locID = model.locID; // newLoc

                                    tranDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));
                                    var insertOutQuery = $"INSERT INTO KPDBA.PTC_STOCK_DETAIL (TRAN_ID, TRAN_SEQ, TRAN_TYPE, TRAN_DATE,PTC_ID, QTY, COMP_ID, WAREHOUSE_ID, LOC_ID, STATUS, CR_DATE, CR_ORG_ID, CR_USER_ID, PTC_TYPE) VALUES ('{tran_id}', TO_NUMBER('{tranSEQ}'), TO_NUMBER('{tranType}'), TO_DATE('{tranDate}', 'dd/mm/yyyy hh24:mi:ss'),'{model.ptcID}', TO_NUMBER('{QTY}'), TO_CHAR('{compID}'),'{WH}','{locID}', TO_CHAR('{S_STATUS}'), SYSDATE, '{userProfile.org}', '{userProfile.userID}', TO_CHAR('{ptcTypes}'))";
                                    insertQuery.Add(insertOutQuery);
                                    var resultInsertAll = await new DataContext().ExecuteDapperMultiAsync(DataBaseHostEnum.KPR, insertQuery);
                                }
                            }
                        }
                        else
                        {
                            _returnFlag = "1";
                            //ไม่พบหมายเลข Location นี้ในฐานข้อมูล
                            _returnText = "ไม่พบพื้นจัดเก็บที่นี้ภายในคลัง";
                        }
                    }
                    else
                    {
                        _returnFlag = "1";
                        _returnText = "ไม่พบรหัส Tooling นี้ในฐานข้อมูล";
                    }
                }
                else
                {
                    _returnFlag = "1";
                    _returnText = "ข้อมูลผู้ใช้เกิดข้อผิดพลาด กรุณาติดต่อฝ่าย IT";
                }
            }
            else
            {
                _returnFlag = "1";
                _returnText = "ไม่พบข้อมูลผู้ใช้ กรุณา Login อีกครั้ง หรือติดต่อฝ่าย IT ";
            }
            var retuenResult = new ReturnDataMoveLoc
            {
                flag = _returnFlag,
                text = _returnText
            };
            return retuenResult;
        }
        // private async void InsertTostockDetail () {

        // }

        //* Check Warehouse & Warehouse User
        public async Task<object> checkWareHouse(RequestAllModelsPTC model)
        {
            string _returnFlag = "1";
            string _returnText = "error: คุณไม่มีสิทธิ์ในการใช้งานนี้";
            List<WarehouseList> _resultWarehouse = null;
            Object _userTool = "";

            if (model.token != null)
            {
                UserProfile userProfile = _jwtGenerator.DecodeToken(model.token);
                var _userWarehouse = await new StoreConnectionMethod(_mapper).PtcGetWareHouse(userProfile.aduserID);
                var wareHouse = _mapper.Map<IEnumerable<UserWareHouseID>>(_userWarehouse);
                _resultWarehouse = _mapper.Map<IEnumerable<UserWareHouseID>, IEnumerable<WarehouseList>>(wareHouse).ToList();
                decimal count = (_userWarehouse as List<dynamic>).Count;

                switch (count)
                {
                    case 1:
                        _returnFlag = "0";
                        _returnText = "ที่เดียว";
                        break;
                    case 2:
                        _returnFlag = "2";
                        _returnText = "หลายที่";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                _returnText = "ไม่พบข้อมูลผู้ใช้ กรุณา Login อีกครั้ง หรือติดต่อฝ่าย IT ";
            }

            var returnResult = new CheckWareHouseUser
            {
                warehouseList = _resultWarehouse,
                flag = _returnFlag,
                text = _returnText
            };
            return returnResult;
        }
        public async Task<object> checkToolingWareHouse()
        {
            var query = $"SELECT PTC_TYPE, PTC_DESC FROM KPDBA.PTC_TYPE_MASTER WHERE CANCEL_FLAG = 'F'";
            var result = await new DataContext().GetResultDapperAsyncObject(DataBaseHostEnum.KPR, query);
            var results = _mapper.Map<IEnumerable<WareHouseTooling>>(result);
            var _result = _mapper.Map<IEnumerable<WareHouseTooling>, IEnumerable<WareHouseToolingNew>>(results).ToList();
            return _result;
        }
    }
}