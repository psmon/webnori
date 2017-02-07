DROP TRIGGER IF EXISTS tr_tblIndianHodelList_update;
DELIMITER //
CREATE TRIGGER tr_tblIndianHodelList_update
  BEFORE UPDATE
  ON tblindianholdemlist
  FOR EACH ROW
BEGIN      
  DECLARE _creatorGamePoint INT;
  
  if OLD.mainType = 0 then
  begin
    if OLD.gameState =0 and NEW.gameState = 1 then
    begin
    
      select gamePoint into _creatorGamePoint from tblbankinfo where memberID = OLD.creatorID;  
      set _creatorGamePoint = _creatorGamePoint + New.lockPoint;
      update tblbankinfo 
        set gamePoint = _creatorGamePoint 
        where memberID = OLD.creatorID;    
      
      set New.lockPoint = 0;
      
    end;
    end if;  
  end;
  end if;  
  
END //
DELIMITER;
