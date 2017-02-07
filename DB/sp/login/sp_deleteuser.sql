DROP PROCEDURE IF EXISTS sp_deleteuser;
DELIMITER $$
CREATE PROCEDURE sp_deleteuser (
  IN socialType INT,
  IN socialID VARCHAR(45)
  )
BEGIN  
  #Result Value    
  DECLARE Result INT;
  #tmp Value
  
  DECLARE usercount INT;
  DECLARE _memberid INT;
  select COUNT(*),memberID into usercount,_memberid from tblmember where tblmember.socialType = socialType and tblmember.socialID = socialID;
  IF usercount>0 THEN
  begin
    delete from tblmember where tblmember.socialType = socialType and tblmember.socialID = socialID;    
    SET Result = 0;
  end;
  ELSE
    SET Result = -1;
  END IF;
  
  SELECT Result;
  
END $$
DELIMITER ;

