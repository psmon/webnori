CREATE TABLE `tbllog_delmember` (
  `memberID` int(11) NOT NULL,
  `email` varchar(45) NOT NULL,
  `nick` varchar(45) NOT NULL,
  `gamePoint` int(11) NOT NULL,
  `coinCount` int(11) NOT NULL,
  PRIMARY KEY (`memberID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
