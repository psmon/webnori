CREATE TABLE `tblmember` (
  `memberID` int(11) NOT NULL AUTO_INCREMENT,
  `socialType` int(11) NOT NULL COMMENT '0=my db\n1=facebook\n2=kakao\n',
  `socialID` varchar(45) NOT NULL,
  `nick` varchar(45) NOT NULL,
  `picurl` varchar(45) DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  `pw` varchar(45) DEFAULT NULL,
  `createtime` int(11) DEFAULT NULL,
  PRIMARY KEY (`memberID`)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8;
