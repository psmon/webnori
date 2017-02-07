DROP PROCEDURE IF EXISTS sp_createuser;
DELIMITER $$
CREATE PROCEDURE sp_createuser (
  IN socialType INT,
  IN socialID VARCHAR(45),
  IN nick VARCHAR(45),
  IN picurl VARCHAR(45),
  IN email VARCHAR(45),
  IN pw VARCHAR(45)  
  )
BEGIN
  #Result Value    
  DECLARE Result INT;
  
  #Tmp Value
  DECLARE usercount INT;
  DECLARE sameNickCount INT DEFAULT 0;
  DECLARE _currentTime INT;
  
  Select UNIX_TIMESTAMP(current_timestamp) into _currentTime;
  select COUNT(*) into usercount from tblmember where tblmember.socialType = socialType and tblmember.socialID = socialID;
  
  if socialType=0 then
    select COUNT(*) into sameNickCount from tblmember where tblmember.nick = nick;
  end if;
  
  
  IF sameNickCount > 0 THEN
    SET Result = -3;
  ELSEIF usercount>0 THEN
    SET Result = -2;
  ELSE
  begin
    insert into tblmember (
       memberID
      ,socialType
      ,socialID
      ,nick
      ,picurl
      ,email
      ,pw
      ,createtime
    ) VALUES (
       NULL -- memberID - IN int(11)
      ,socialType -- socialType - IN int(11)
      ,socialID -- socialID - IN varchar(45)
      ,nick -- nick - IN varchar(45)
      ,picurl  -- picurl - IN varchar(45)
      ,email  -- email - IN varchar(45)
      ,pw  -- pw - IN varchar(45)
      ,_currentTime
    );
    SET Result = 0;
  end;
  END IF;
  
  SELECT Result;
  
END $$
DELIMITER ;

