CREATE TABLE `tblhandgamedetail` (
  `gameID` int(11) NOT NULL,
  `subGameID` int(11) NOT NULL,
  `gameFlag_right1` varchar(45) NOT NULL,
  `memberID_right` int(11) NOT NULL,
  `memberNick_right` varchar(45) NOT NULL,
  `gameMemo_right` varchar(200) NOT NULL,
  `winnerID` int(11) NOT NULL,
  `earnPoint` int(11) NOT NULL,
  `joinTime` int(11) DEFAULT NULL,
  PRIMARY KEY (`gameID`,`subGameID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
