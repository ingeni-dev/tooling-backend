SELECT COURSE_TYPE,
       COURSE_ID,
       COURSE_REVISION,
       GTD.GROUP_ID,
       TOPIC_NAME GROUP_NAME,
       GROUP_ORDER
  FROM (SELECT DOC_TYPE COURSE_TYPE,
               COURSE_ID,
               COURSE_REVISION,
               TOPIC_ID GROUP_ID,
               TOPIC_ORDER GROUP_ORDER
          FROM KPDBA.COURSE_TOPIC
         WHERE     COURSE_ID = :AD_COURSE_ID
               AND COURSE_REVISION = :AD_COURSE_REVISION
               AND CANCEL_FLAG = 'F') GTD,
       (SELECT TOPIC_ID,
               TOPIC_ORDER,
               TOPIC_NAME,
               PARENT_TOPIC_ID GROUP_ID
          FROM KPDBA.TOPIC_MASTER) TM
 WHERE GTD.GROUP_ID = TM.TOPIC_ID ORDER BY GROUP_ORDER ASC