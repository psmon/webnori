DROP PROCEDURE IF EXISTS sp_rungame;
DELIMITER $$
CREATE PROCEDURE sp_rungame (
  IN gameID INT,
  IN gameFlag_right1 VARCHAR(45),
  IN playerAuthToken VARCHAR(45),
  IN userIP VARCHAR(45), 
  IN gameMemo_right VARCHAR(200),
  IN whoWinner INT #0:left , 1:right
  )
BEGIN  
  DECLARE Result INT;
  DECLARE usercount INT;  
  DECLARE subGameID INT;
  DECLARE nextGameID INT;
  DECLARE gameCount INT;
  DECLARE memberID_right INT;
  DECLARE memberNick_right VARCHAR(45);
  DECLARE winnerID INT;
  DECLARE earnPoint INT;  
  DECLARE MemberID INT;
  DECLARE gamePoint INT;
  DECLARE gameType INT;
  DECLARE gameFlag_left1 VARCHAR(45);
  DECLARE MemberID_left INT;  
  
  DECLARE gamePoint_left INT;
  DECLARE lockPoint_left INT;
  
  DECLARE gamePoint_right INT;
  DECLARE cur_gameState INT;
  DECLARE publishCnt INT;
  DECLARE currentTime INT;
  
  
  DECLARE EXIT HANDLER FOR SQLEXCEPTION 
  BEGIN
    ROLLBACK;        
    set Result = -100;
    SELECT Result;
    SHOW WARNINGS; #DIAGNOSTICS    
  END;
  
  START TRANSACTION;
  
  Select UNIX_TIMESTAMP(current_timestamp) into currentTime;  
  
  SET Result = 0;
  
  select tblHandGameList.curGameIdx,tblHandGameList.gameMoneyAmount,tblHandGameList.gameCount,tblHandGameList.gameType,
          tblHandGameList.gameFlag_left1,tblHandGameList.MemberID_left,tblHandGameList.lockPoint,tblHandGameList.gameState
    into subGameID,earnPoint,gameCount,gameType,gameFlag_left1,MemberID_left,lockPoint_left,cur_gameState from tblHandGameList where tblHandGameList.gameID = gameID;  
  
  select tblloginstate.memberid 
    into MemberID from tblloginstate where tblloginstate.authtoken = playerAuthToken;
  
  select tblbankinfo.gamePoint 
    into gamePoint from tblbankinfo where tblbankinfo.memberID = MemberID;
    
  select tblmember.nick into memberNick_right FROM tblmember where tblmember.memberID = MemberID;      
    
  select Count(*) into publishCnt FROM tblHandGameDetail where tblHandGameDetail.gameID = gameID and tblHandGameDetail.memberID_right = MemberID;
      
  #Error Check
  if subGameID=null or MemberID=null then  
    SET Result = -1;  #not exist game or user  
  end if;
  
  if gameCount = subGameID then
    SET Result = -2;  #fullGame
  end if;
  
  if gamePoint < earnPoint then
    SET Result = -3;  #not enoght money
  end if;
  
  if cur_gameState = 1 then
    SET Result = -4;  #completed game
  end if;
  
  if memberID = MemberID_left then
    SET Result = -5;  #same Game
  end if;
  
  if publishCnt > 0 then
    SET Result = -6;
  end if;
      
  if Result = 0 then
  begin
    set nextGameID = subGameID+1;
    update tblHandGameList SET curGameIdx = nextGameID where tblHandGameList.gameID = gameID;
    
    IF whoWinner=1 THEN
    BEGIN
      SET winnerID = MemberID_left;
      set lockPoint_left =lockPoint_left + earnPoint;
      update tblHandGameList SET lockPoint=lockPoint_left where tblHandGameList.gameID = gameID;
      
      set gamePoint_right = gamePoint - earnPoint;
      
      update tblbankinfo SET gamePoint = gamePoint_right, loseCount=loseCount+1 where tblbankinfo.memberID = MemberID;              
      update tblbankinfo SET winCount=winCount+1 where tblbankinfo.memberID = MemberID_left;
      
      insert into tblgamehistory(gameType,gameNo,gameSubNo,winnerID,loseID,betAmount,joinTime,userIP)
        values(0,gameID,subGameID,winnerID,MemberID,earnPoint,currentTime,userIP);
      
      
    END;
    ELSEIF  whoWinner=2 THEN
    BEGIN
      SET winnerID = MemberID;      
      set lockPoint_left =lockPoint_left - earnPoint;
      update tblHandGameList SET lockPoint=lockPoint_left where tblHandGameList.gameID = gameID;
      set gamePoint_right = gamePoint + earnPoint;
      update tblbankinfo SET gamePoint = gamePoint_right,winCount = winCount+1  where tblbankinfo.memberID = MemberID;              
      update tblbankinfo SET loseCount=loseCount+1 where tblbankinfo.memberID = MemberID_left;              
      
      insert into tblgamehistory(gameType,gameNo,gameSubNo,winnerID,loseID,betAmount,joinTime,userIP)
        values(0,gameID,subGameID,winnerID,MemberID_left,earnPoint,currentTime,userIP);
      
    END;
    ELSE
    BEGIN
      #draw game...
      SET winnerID = 0;
      SET earnPoint = 0;
    END;
    END IF; 
    
    INSERT INTO tblHandGameDetail(
      gameID,subGameID,gameFlag_right1,memberID_right,memberNick_right,
      gameMemo_right,winnerID,earnPoint,joinTime
    )
    VALUES(
      gameID,subGameID,gameFlag_right1,MemberID,memberNick_right,
      gameMemo_right,winnerID,earnPoint,currentTime
    );
  end;
  end if;
  
  
  #After Update ( complete Game )
  if nextGameID = gameCount and Result=0 then
  begin            
    update tblHandGameList SET gameState = 1 where tblHandGameList.gameID = gameID;
  end;
  end if;
  
  SELECT Result;
  COMMIT;
        
END $$
DELIMITER ;

