﻿SELECT EMP_ID, EMP_FNAME||' '||EMP_LNAME EMP_NAME FROM KPDBA.EMPLOYEE WHERE RESIGN_DATE IS NULL AND EMP_ID = :AS_EMP_ID