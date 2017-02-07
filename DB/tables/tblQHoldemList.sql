CREATE TABLE `tblQHoldemList` (
  `gameID` int(11) NOT NULL AUTO_INCREMENT,
  `gameState` int(11) NOT NULL DEFAULT '0',
  `gameTitle` varchar(50) NOT NULL,
  `creatorID` int(11) NOT NULL,
  `creatorNick` varchar(20) NOT NULL,
  `gameData` varchar(20) NOT NULL,
  `gameCount` int(11) NOT NULL,
  `curGameIdx` int(11) NOT NULL DEFAULT '-1',
  `gameMoneyAmount` int(11) NOT NULL,
  `createTime` int(11) NOT NULL,
  `creatorAvatar` varchar(45) NOT NULL,
  `lockPoint` int(11) NOT NULL,
  `mainType` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`gameID`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8;
