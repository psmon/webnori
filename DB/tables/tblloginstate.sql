CREATE TABLE `tblloginstate` (
  `memberid` int(11) NOT NULL,
  `authtoken` varchar(45) DEFAULT NULL,
  `lastlogintime` int(20) DEFAULT NULL,
  PRIMARY KEY (`memberid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
