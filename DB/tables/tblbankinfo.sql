CREATE TABLE `tblbankinfo` (
  `memberID` int(11) NOT NULL,
  `gamePoint` int(11) NOT NULL,
  `coinCount` int(11) NOT NULL,
  `winCount` int(11) NOT NULL DEFAULT '0',
  `loseCount` int(11) NOT NULL DEFAULT '0',
  `lastBonusUpdateTime` int(11) DEFAULT '0',
  `lastAdwardBonusTime` int(11) DEFAULT '0',
  PRIMARY KEY (`memberID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
