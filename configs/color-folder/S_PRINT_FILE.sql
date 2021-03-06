  SELECT SUBSTR (PROD_ID, 7) || '-' || REVISION PROD_ID, PROD_DESC
    FROM KPDBA.PRODUCT
   WHERE     (   PROD_DESC LIKE '%' || :AS_TXT_SEARCH || '%'
              OR PROD_ID LIKE '%' || :AS_TXT_SEARCH || '%')
         AND CANCEL_FLAG = 'F' AND ROWNUM <= 25
ORDER BY PROD_DESC ASC