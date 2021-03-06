﻿SELECT LOC_ID,
       LOC_DETAIL,
       CF.WAREHOUSE_ID,
       COMP_ID
  FROM KPDBA.LOCATION_CF CF, KPDBA.WAREHOUSE_CF WH
 WHERE     CF.WAREHOUSE_ID = WH.WAREHOUSE_ID
       AND CF.CANCEL_FLAG = 'F'
       AND LOC_ID = :AS_LOC_ID
       AND CF.WAREHOUSE_ID = :AS_WAREHOUSE_ID