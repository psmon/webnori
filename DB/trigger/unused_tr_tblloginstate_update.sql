DROP TRIGGER IF EXISTS tr_tblloginstate_update;
DELIMITER //
CREATE TRIGGER tr_tblloginstate_update
  BEFORE UPDATE
  ON tblloginstate
  FOR EACH ROW
BEGIN      
  DECLARE gamePoint INT;
  DECLARE gameCoin INT;
  DECLARE lastBonusUpdateTime INT;
  DECLARE isChangeData BOOL DEFAULT False;
  DECLARE dayOffSet INT;
  
  set dayOffSet = 60 * 60 * 24;
  
  set isChangeData = (OLD.lastlogintime = NEW.lastlogintime);
  
  if isChangeData=True then
    select tblbankinfo.gamePoint,tblbankinfo.coinCount,tblbankinfo.lastBonusUpdateTime into gamePoint,gameCoin,lastBonusUpdateTime from tblbankinfo 
      where tblbankinfo.memberID = OLD.memberid;    
    if lastBonusUpdateTime + dayOffSet < NEW.lastlogintime then
    begin
      #day bonus..
      set gamePoint=gamePoint+100;
      set gameCoin=gameCoin+1;
      set lastBonusUpdateTime = NEW.lastlogintime;
      update tblbankinfo set tblbankinfo.gamePoint = gamePoint,tblbankinfo.coinCount = gameCoin,
        tblbankinfo.lastBonusUpdateTime = lastBonusUpdateTime where tblbankinfo.memberID = OLD.memberid;              
    end;
    end if;
    
  begin
  end;
  end if;
  
END //
DELIMITER;
