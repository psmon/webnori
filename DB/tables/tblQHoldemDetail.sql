CREATE TABLE `tblQHoldemDetail` (
  `gameID` int(11) NOT NULL,
  `subGameID` int(11) NOT NULL,
  `joinerID` int(11) NOT NULL,
  `joinerNick` varchar(20) NOT NULL,
  `joinerAvatar` varchar(45) NOT NULL,
  `joinerMemo` varchar(50) NOT NULL,
  `joinerGameData` int(11) NOT NULL,
  `commCardData` varchar(20) NOT NULL,
  `earnPoint` int(11) NOT NULL,
  `winnerID` int(11) NOT NULL,
  `joinTime` int(11) DEFAULT '0',  
  `creatorHandStr` varchar(100) NOT NULL DEFAULT 'nan',
  `joinerHandStr` varchar(100) NOT NULL DEFAULT 'nan',
  PRIMARY KEY (`gameID`,`subGameID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
