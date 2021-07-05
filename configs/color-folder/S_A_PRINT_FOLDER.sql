﻿SELECT C.CUST_NAME,
       P.PROD_ID,
       P.PROD_DESC,
       TO_CHAR (CF.RETURN_DATE, 'DD/MM/YYYY') APP_DATE,
       TO_CHAR (CF.EXPIRE_DATE, 'DD/MM/YYYY') EXPIRE_DATE,
       CASE
          WHEN SN.CF_NO IS NULL THEN NULL
          ELSE TO_CHAR (SN.CF_NO) || '/' || TO_CHAR (CF.SUBMIT_QTY)
       END
          RUNNING_NO,
       ' ' APPROVAL,
       SN.CF_SN
  FROM KPDBA.COLOR_FOLDER CF
       JOIN KPDBA.COLOR_FOLDER_SN SN
          ON CF.CF_SEQ = SN.CF_SEQ
       JOIN KPDBA.PRODUCT P
          ON CF.PROD_ID = P.PROD_ID AND CF.REVISION = P.REVISION
       JOIN KPDBA.PROD_CUST PC
          ON CF.PROD_ID = PC.PROD_ID AND CF.REVISION = PC.REVISION
       JOIN KPDBA.CUSTOMER C
          ON PC.CUST_ID = C.CUST_ID
 WHERE SN.CF_SN = :AS_CF_SN AND ROWNUM <= 1