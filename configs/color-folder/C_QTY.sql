﻿SELECT SUM (QTY) AS REMAIN_QTY
FROM KPDBA.CF_STOCK_DETAIL
WHERE STATUS = 'T' AND CF_SN = :S_CF_SN GROUP BY CF_SN