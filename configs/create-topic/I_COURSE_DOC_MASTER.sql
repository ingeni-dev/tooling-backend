INSERT INTO KPDBA.COURSE_DOCUMENT_MASTER (COURSE_DOC_ID,
                                          DOC_TYPE,
                                          DOC_NAME,
                                          DOC_PATH,
                                          VIDEO_COVER,
                                          VIDEO_LENGTH,
                                          CR_DATE,
                                          CR_ORG_ID,
                                          CR_USER_ID,
                                          CANCEL_FLAG)
     VALUES (:AD_COURSE_DOC_ID,
             :AD_DOC_TYPE,
             :AD_DOC_NAME,
             :AD_DOC_PATH,
             :AD_VIDEO_COVER,
             TO_NUMBER (:AD_VIDEO_LENGTH),
             SYSDATE,
             TO_CHAR (:AD_ORG_ID),
             TO_CHAR (:AD_USER_ID),
             :AD_CANCEL_FLAG)