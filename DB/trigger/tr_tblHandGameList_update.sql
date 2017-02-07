DROP TRIGGER IF EXISTS tr_tblHandGameList_update;
DELIMITER //
CREATE TRIGGER tr_tblHandGameList_update
  BEFORE UPDATE
  ON tblHandGameList
  FOR EACH ROW
BEGIN      
  DECLARE gamePoint_left INT;
  
  if OLD.mainType = 0 then
  begin
    if OLD.gameState =0 and NEW.gameState = 1 then
    begin
      select tblbankinfo.gamePoint into gamePoint_left from tblbankinfo where tblbankinfo.memberID = OLD.memberID_left;  
      set gamePoint_left = gamePoint_left + New.lockPoint;
      update tblbankinfo SET gamePoint = gamePoint_left where tblbankinfo.memberID = OLD.memberID_left;
      set New.lockPoint = 0;
    end;
    end if; 
  end;
  end if;    
  
END //
DELIMITER;
