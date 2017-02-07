DROP PROCEDURE IF EXISTS sp_creategame;
DELIMITER $$
CREATE PROCEDURE sp_creategame (
  IN mainType INT,        #0:private game , 1:publicgame , 2:spon game
  IN gameType INT,        #0:gawibawi,1:indian holdem
  IN authType INT,        #0:anoymous , 1:auth
  IN gameState INT,       #0:new,1:complete
  IN gameTitle VARCHAR(200),
  IN gameMemo_left VARCHAR(200),
  IN gameToken VARCHAR(45),  
  IN gameCount INT,
  IN gameMoneyAmount INT,
  IN gameFlag_left1 VARCHAR(200),    
  IN addendtime INT,    
  IN creatorAuthToken VARCHAR(45)
  )
BEGIN    
  #Result Value    
  DECLARE Result INT;
  
  #Tmp Value  
  DECLARE sameNickCount INT;
  DECLARE createtime INT;  
  DECLARE endtime INT;  
  DECLARE curGameIdx INT;  
  DECLARE memberID_left INT;
  DECLARE gamePoint INT;
  DECLARE lockPoint INT;
  DECLARE publishCnt INT;
  
  
  DECLARE memberNick_left VARCHAR(45);
  
  DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN
    ROLLBACK;    
    set Result = -100;
    SELECT Result;    
    SHOW WARNINGS; #DIAGNOSTICS    
  END;
  
  START TRANSACTION;
  
  set Result = 0;  
  set curGameIdx = 0;        
  Select UNIX_TIMESTAMP(current_timestamp) into createtime;  
  set endtime = createtime + addendtime;
  
  select tblloginstate.memberid into memberID_left from tblloginstate where tblloginstate.authtoken = creatorAuthToken;
  
  select tblbankinfo.gamePoint into gamePoint from tblbankinfo where tblbankinfo.memberID = memberID_left;
  
    
  select Count(*) into publishCnt from tblHandGameList where tblHandGameList.memberID_left = memberID_left and tblHandGameList.gameState =0 and tblHandGameList.mainType=0;
  
    
  IF memberID_left = NULL THEN
    set Result = -1;
    ROLLBACK;    
  END IF;
  
  if mainType=0 then
  begin
    IF gamePoint < gameMoneyAmount*gameCount THEN
      set Result = -2;
      ROLLBACK;    
    END IF;
    
    IF 3 < publishCnt Then
      set Result = -3;
      ROLLBACK;    
    END IF;
  end;
  end if;
        
    
  IF Result=0 THEN
  BEGIN
    set gamePoint = gamePoint - gameMoneyAmount*gameCount;
    set lockPoint = gameMoneyAmount*gameCount;
    
    if mainType=0 then
      update tblbankinfo set tblbankinfo.gamePoint = gamePoint where tblbankinfo.memberID = memberID_left;      
    end if;    
    
    select tblmember.nick into memberNick_left from tblmember where tblmember.memberID = memberID_left;    
  
    insert into tblHandGameList( 
    gameID, mainType,gameType,authType,gameState, gameTitle, memberID_left, 
    memberNick_left, gameMemo_left, gameToken, gameCount, curGameIdx, 
    gameMoneyAmount,lockPoint,gameFlag_left1, createtime, endtime 
    )
    values(
      NULL,mainType,gameType, authType,gameState, gameTitle, memberID_left, 
      memberNick_left, gameMemo_left, gameToken, gameCount, curGameIdx, 
      gameMoneyAmount, lockPoint,gameFlag_left1, createtime, endtime            
    );
    COMMIT;    
  END;
  END IF;    
  
  
  SELECT Result;
  
  
  
END $$
DELIMITER ;

