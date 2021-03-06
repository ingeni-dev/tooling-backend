using PTCwebApi.Models.Elearning;
using PTCwebApi.Models.PTCModels.MethodModels;
using webAPI.Models.Elearning;

namespace webAPI.MapperProfile
{
    public class ElearnMapper : AutoMapper.Profile
    {
        public ElearnMapper()
        {
            CreateMap<ResponseCourseDetails, ResultCourseDetails>()
               .ForMember(d => d.docType, o => o.MapFrom(s => s.DOC_TYPE))
               .ForMember(d => d.courseID, o => o.MapFrom(s => s.COURSE_ID))
               .ForMember(d => d.courseDESC, o => o.MapFrom(s => s.COURSE_DESC))
               .ForMember(d => d.queryID, o => o.MapFrom(s => s.QUERY_ID))
               .ForMember(d => d.place, o => o.MapFrom(s => s.PLACE))
               .ForMember(d => d.remark, o => o.MapFrom(s => s.REMARK))
               .ForMember(d => d.timeSeq, o => o.MapFrom(s => s.TIME_SEQ))
               .ForMember(d => d.lectName, o => o.MapFrom(s => s.LECT_NAME))
               .ForMember(d => d.applicantCount, o => o.MapFrom(s => s.APPLICANT_COUNT))
               .ForMember(d => d.queryBegin, o => o.MapFrom(s => s.QUERY_BEGIN))
               .ForMember(d => d.queryEnd, o => o.MapFrom(s => s.QUERY_END))
               .ForMember(d => d.dayHour, o => o.MapFrom(s => s.DAY_HOUR))
               .ForMember(d => d.dayMin, o => o.MapFrom(s => s.DAY_MIN))
               .ForMember(d => d.trainTypeID, o => o.MapFrom(s => s.TRAIN_TYPE_ID))
               .ForMember(d => d.trainTypeDESC, o => o.MapFrom(s => s.TRAIN_TYPE_DESC))
               .ForMember(d => d.instantFlag, o => o.MapFrom(s => s.INSTANT_FLAG));

            CreateMap<CourseFromMap, CourseFrom>()
               .ForMember(d => d.courseDESC, o => o.MapFrom(s => s.COURSE_DESC));

            CreateMap<GetTrainType, SetTrainType>()
               .ForMember(d => d.trainTypeID, o => o.MapFrom(s => s.TRAIN_TYPE_ID))
               .ForMember(d => d.trainTypeDESC, o => o.MapFrom(s => s.TRAIN_TYPE_DESC));

            CreateMap<ApplicantDetailResponse, ApplicantDetailResult>()
               .ForMember(d => d.docType, o => o.MapFrom(s => s.DOC_TYPE))
               .ForMember(d => d.queryID, o => o.MapFrom(s => s.QUERY_ID))
               .ForMember(d => d.timeSeq, o => o.MapFrom(s => s.TIME_SEQ))
               .ForMember(d => d.appEmpID, o => o.MapFrom(s => s.APP_EMP_ID))
               .ForMember(d => d.empName, o => o.MapFrom(s => s.EMP_NAME))
               .ForMember(d => d.trainingFlag, o => o.MapFrom(s => s.TRAINING_FLAG));
            //    .ForMember(d => d.unitID, o => o.MapFrom(s => s.UNIT_ID))
            //    .ForMember(d => d.unitDesc, o => o.MapFrom(s => s.UNIT_DESC));

            CreateMap<GetAllCoursesDB, SetAllCoursesDB>()
               .ForMember(d => d.docType, o => o.MapFrom(s => s.DOC_TYPE))
               .ForMember(d => d.docID, o => o.MapFrom(s => s.DOC_ID))
               .ForMember(d => d.docRev, o => o.MapFrom(s => s.DOC_REV))
               .ForMember(d => d.docDesc, o => o.MapFrom(s => s.COURSE_DESC))
               .ForMember(d => d.remark, o => o.MapFrom(s => s.REMARK));

            CreateMap<GetAllISOsDB, SetAllISOsDB>()
               .ForMember(d => d.docType, o => o.MapFrom(s => s.DOC_TYPE))
               .ForMember(d => d.docCode, o => o.MapFrom(s => s.DOC_CODE))
               .ForMember(d => d.docRevision, o => o.MapFrom(s => s.DOC_REVISION))
               .ForMember(d => d.docName, o => o.MapFrom(s => s.DOC_NAME))
               .ForMember(d => d.isoSTD, o => o.MapFrom(s => s.ISO_STD));

            CreateMap<GetTopicGroup, SetTopicGroup>()
               .ForMember(d => d.courseType, o => o.MapFrom(s => s.COURSE_TYPE))
               .ForMember(d => d.courseID, o => o.MapFrom(s => s.COURSE_ID))
               .ForMember(d => d.courseRevision, o => o.MapFrom(s => s.COURSE_REVISION))
               .ForMember(d => d.groupID, o => o.MapFrom(s => s.GROUP_ID))
               .ForMember(d => d.groupName, o => o.MapFrom(s => s.GROUP_NAME))
               .ForMember(d => d.groupOrder, o => o.MapFrom(s => s.GROUP_ORDER));

            CreateMap<GetTopic, SetTopic>()
               .ForMember(d => d.topicID, o => o.MapFrom(s => s.TOPIC_ID))
               .ForMember(d => d.topicOrder, o => o.MapFrom(s => s.TOPIC_ORDER))
               .ForMember(d => d.topicName, o => o.MapFrom(s => s.TOPIC_NAME))
               .ForMember(d => d.groupID, o => o.MapFrom(s => s.GROUP_ID));

            CreateMap<GetDoc, SetDoc>()
               .ForMember(d => d.topicID, o => o.MapFrom(s => s.TOPIC_ID))
               .ForMember(d => d.docID, o => o.MapFrom(s => s.COURSE_DOC_ID))
               .ForMember(d => d.docOrder, o => o.MapFrom(s => s.DOC_ORDER))
               .ForMember(d => d.docName, o => o.MapFrom(s => s.DOC_NAME))
               .ForMember(d => d.docType, o => o.MapFrom(s => s.DOC_TYPE))
               .ForMember(d => d.docPath, o => o.MapFrom(s => s.DOC_PATH))
               .ForMember(d => d.videoCover, o => o.MapFrom(s => s.VIDEO_COVER))
               .ForMember(d => d.videoLength, o => o.MapFrom(s => s.VIDEO_LENGTH));

            CreateMap<GetAllLectureMap, GetAllLecture>()
               .ForMember(d => d.type, o => o.MapFrom(s => s.TYPE))
               .ForMember(d => d.lectID, o => o.MapFrom(s => s.LECT_ID))
               .ForMember(d => d.thaiTitle, o => o.MapFrom(s => s.THAI_TITLE))
               .ForMember(d => d.thaiFname, o => o.MapFrom(s => s.THAI_FNAME))
               .ForMember(d => d.thaiSname, o => o.MapFrom(s => s.THAI_SNAME))
               .ForMember(d => d.refEmpID, o => o.MapFrom(s => s.REF_EMP_ID))
               .ForMember(d => d.thaiName, o => o.MapFrom(s => s.THAI_NAME));


            CreateMap<GetAllApplicantMap, GetAllApplicant>()
               .ForMember(d => d.empID, o => o.MapFrom(s => s.EMP_ID))
               .ForMember(d => d.empFname, o => o.MapFrom(s => s.EMP_FNAME))
               .ForMember(d => d.empLname, o => o.MapFrom(s => s.EMP_LNAME))
               .ForMember(d => d.posID, o => o.MapFrom(s => s.POS_ID))
               .ForMember(d => d.posDESC, o => o.MapFrom(s => s.POS_DESC))
               .ForMember(d => d.roleID, o => o.MapFrom(s => s.ROLE_ID))
               .ForMember(d => d.roleDESC, o => o.MapFrom(s => s.ROLE_DESC))
               .ForMember(d => d.trainingFlag, o => o.MapFrom(s => s.TRAINING_FLAG))
               .ForMember(d => d.selectedFlag, o => o.MapFrom(s => s.SELECTED_FLAG_BOOL));

            CreateMap<GetALLOnlineCourse, SetALLOnlineCourse>()
               .ForMember(d => d.queryID, o => o.MapFrom(s => s.QUERY_ID))
               .ForMember(d => d.courseID, o => o.MapFrom(s => s.COURSE_ID))
               .ForMember(d => d.courseRevision, o => o.MapFrom(s => s.COURSE_REVISION))
               .ForMember(d => d.courseDESC, o => o.MapFrom(s => s.COURSE_DESC))
               .ForMember(d => d.beginDate, o => o.MapFrom(s => s.BEGIN_DATE))
               .ForMember(d => d.endDate, o => o.MapFrom(s => s.END_DATE))
               .ForMember(d => d.fullTimeCourse, o => o.MapFrom(s => s.FULL_TIME_COURSE))
               .ForMember(d => d.precentViewCourse, o => o.MapFrom(s => s.PERCENT_VIEW_COURSE));

            CreateMap<GetALLOnlineCourseLatest, SetALLOnlineCourseLatest>()
               .ForMember(d => d.appUserID, o => o.MapFrom(s => s.APP_USER_ID))
               .ForMember(d => d.queryID, o => o.MapFrom(s => s.QUERY_ID))
               .ForMember(d => d.courseDESC, o => o.MapFrom(s => s.COURSE_DESC))
               .ForMember(d => d.beginDate, o => o.MapFrom(s => s.BEGIN_DATE))
               .ForMember(d => d.endDate, o => o.MapFrom(s => s.END_DATE))
               .ForMember(d => d.courseDocID, o => o.MapFrom(s => s.COURSE_DOC_ID))
               .ForMember(d => d.courseID, o => o.MapFrom(s => s.COURSE_ID))
               .ForMember(d => d.courseRevision, o => o.MapFrom(s => s.COURSE_REVISION))
               .ForMember(d => d.topicID, o => o.MapFrom(s => s.TOPIC_ID))
               .ForMember(d => d.cancelFlag, o => o.MapFrom(s => s.CANCEL_FLAG))
               .ForMember(d => d.cTopicID, o => o.MapFrom(s => s.C_TOPIC_ID))
               .ForMember(d => d.cTopicOrder, o => o.MapFrom(s => s.C_TOPIC_ORDER))
               .ForMember(d => d.cTopicName, o => o.MapFrom(s => s.C_TOPIC_NAME))
               .ForMember(d => d.cParentTopicID, o => o.MapFrom(s => s.C_PARENT_TOPIC_ID))
               .ForMember(d => d.tTopicID, o => o.MapFrom(s => s.T_TOPIC_ID))
               .ForMember(d => d.tTopicOrder, o => o.MapFrom(s => s.T_TOPIC_ORDER))
               .ForMember(d => d.tTopicName, o => o.MapFrom(s => s.T_TOPIC_NAME))
               .ForMember(d => d.tParentTopicID, o => o.MapFrom(s => s.T_PARENT_TOPIC_ID))
               .ForMember(d => d.docType, o => o.MapFrom(s => s.DOC_TYPE))
               .ForMember(d => d.docName, o => o.MapFrom(s => s.DOC_NAME))
               .ForMember(d => d.docPath, o => o.MapFrom(s => s.DOC_PATH))
               .ForMember(d => d.videoCover, o => o.MapFrom(s => s.VIDEO_COVER))
               .ForMember(d => d.videoLength, o => o.MapFrom(s => s.VIDEO_LENGTH))
               .ForMember(d => d.count, o => o.MapFrom(s => s.COUNT))
               .ForMember(d => d.currTime, o => o.MapFrom(s => s.CURR_TIME))
               .ForMember(d => d.lastVisit, o => o.MapFrom(s => s.LAST_VISIT));


            CreateMap<GetCourseQueryDoc, SetCourseQueryDoc>()
               .ForMember(d => d.queryDocID, o => o.MapFrom(s => s.QUERY_DOC_ID))
               .ForMember(d => d.count, o => o.MapFrom(s => s.COUNT))
               .ForMember(d => d.currTime, o => o.MapFrom(s => s.CURR_TIME))
               .ForMember(d => d.finishFlag, o => o.MapFrom(s => s.FINISH_FLAG));
        }
    }
}