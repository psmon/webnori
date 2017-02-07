CREATE TABLE `tblgamehistory` (
  `gameType` int(11) NOT NULL,
  `gameNo` int(11) NOT NULL,
  `gameSubNo` int(11) NOT NULL,
  `winnerID` int(11) NOT NULL,
  `loseID` int(11) NOT NULL,
  `betAmount` int(11) NOT NULL,
  `joinTime` int(11) DEFAULT '0',
  PRIMARY KEY (`gameType`,`gameNo`,`gameSubNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
