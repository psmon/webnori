DROP PROCEDURE IF EXISTS sp_earnpointList;
DELIMITER $$
CREATE PROCEDURE sp_earnpointList (  
  IN monthData VARCHAR(45),
  IN gameType INT
  )
BEGIN
  #Result Value
  DECLARE Result INT;  
    
  #Tmp Value   
  DECLARE starttime INT;  
  
  set monthData = CONCAT(monthData ,' 00:00:00');  
  Select UNIX_TIMESTAMP( monthData) into starttime; 
  #Select UNIX_TIMESTAMP(current_timestamp) into currentTime;  
  
  select cast(sum(betAmount) as DECIMAL) as earnPointTotal,winnerID,nick,picurl FROM tblgamehistory
    inner join tblmember on tblgamehistory.winnerID = tblmember.memberID       
    where tblgamehistory.gameType=gameType and starttime<=joinTime group by winnerID ORDER BY earnPointTotal DESC LIMIT 5;
      
  
END $$
DELIMITER ;

