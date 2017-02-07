USE webnori;
DROP TRIGGER IF EXISTS tr_deleteUserAfter;
DELIMITER //
CREATE TRIGGER tr_deleteUserAfter
  AFTER DELETE
  ON tblmember
  FOR EACH ROW
BEGIN

  DECLARE _gamePoint INT;
  DECLARE _coinCount INT;    
  select gamePoint,coinCount into _gamePoint,_coinCount from tblbankinfo where memberID = OLD.memberID;
  DELETE FROM tblbankinfo where memberID = OLD.memberID;
  DELETE FROM tblloginstate where memberid = OLD.memberid;  
  insert into tbllog_delmember values(OLD.memberID,OLD.email,OLD.nick,_gamePoint,_coinCount);
  
END //
DELIMITER;
