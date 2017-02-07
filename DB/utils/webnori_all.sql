-- MySQL dump 10.13  Distrib 5.7.16, for Win64 (x86_64)
--
-- Host: localhost    Database: webnori
-- ------------------------------------------------------
-- Server version	5.7.16-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `tblloginstate`
--

DROP TABLE IF EXISTS `tblloginstate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tblloginstate` (
  `memberid` int(11) NOT NULL,
  `authtoken` varchar(45) DEFAULT NULL,
  `lastlogintime` int(11) DEFAULT NULL,
  PRIMARY KEY (`memberid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tblloginstate`
--

LOCK TABLES `tblloginstate` WRITE;
/*!40000 ALTER TABLE `tblloginstate` DISABLE KEYS */;
INSERT INTO `tblloginstate` VALUES (15,'8cdd59ae-ae57-11e6-a0c9-9a85ca67a2f8',20161119215543);
/*!40000 ALTER TABLE `tblloginstate` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tblmember`
--

DROP TABLE IF EXISTS `tblmember`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tblmember`
--

LOCK TABLES `tblmember` WRITE;
/*!40000 ALTER TABLE `tblmember` DISABLE KEYS */;
INSERT INTO `tblmember` VALUES (15,0,'1133','psmon','http://psmon.x-y.net/a.png','psmon@live.co.kr','123',20161119215351);
/*!40000 ALTER TABLE `tblmember` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `uv_tblmember`
--

DROP TABLE IF EXISTS `uv_tblmember`;
/*!50001 DROP VIEW IF EXISTS `uv_tblmember`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `uv_tblmember` AS SELECT 
 1 AS `nick`,
 1 AS `picurl`*/;
SET character_set_client = @saved_cs_client;

--
-- Dumping routines for database 'webnori'
--
/*!50003 DROP PROCEDURE IF EXISTS `sp_createuser` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_createuser`(

  IN socialType INT,

  IN socialID VARCHAR(45),

  IN nick VARCHAR(45),

  IN picurl VARCHAR(45),

  IN email VARCHAR(45),

  IN pw VARCHAR(45)  

  )
BEGIN  

  DECLARE usercount INT;

  DECLARE _currentTime INT;

  Select current_timestamp into _currentTime;

  select COUNT(*) into usercount from tblmember where tblmember.socialType = socialType and tblmember.socialID = socialID;

  IF usercount>0 THEN

    SELECT -1;

  ELSE

    insert into tblmember (

       memberID

      ,socialType

      ,socialID

      ,nick

      ,picurl

      ,email

      ,pw

      ,createtime

    ) VALUES (

       NULL -- memberID - IN int(11)

      ,socialType -- socialType - IN int(11)

      ,socialID -- socialID - IN varchar(45)

      ,nick -- nick - IN varchar(45)

      ,picurl  -- picurl - IN varchar(45)

      ,email  -- email - IN varchar(45)

      ,pw  -- pw - IN varchar(45)

      ,_currentTime

    );

    SELECT 0;

  END IF;  

  

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_deleteuser` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_deleteuser`(

  IN socialType INT,

  IN socialID VARCHAR(45)

  )
BEGIN  

  DECLARE usercount INT;

  DECLARE _memberid INT;

  select COUNT(*),memberID into usercount,_memberid from tblmember where tblmember.socialType = socialType and tblmember.socialID = socialID;

  IF usercount>0 THEN

    delete from tblmember where tblmember.socialType = socialType and tblmember.socialID = socialID;

    delete from tblloginstate where memberid = _memberid;

    select 0;

  ELSE

    select -1;    

  END IF;  

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_loginuser` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_loginuser`(

  IN _socialType INT,

  IN _socialID VARCHAR(45),

  IN _pw VARCHAR(45)

  )
BEGIN  

  DECLARE _usercount INT;

  DECLARE _memberID INT;  

  DECLARE _authToken VARCHAR(45);

  DECLARE _currentTime INT;

  

  Select current_timestamp into _currentTime;

  #SET _currentTime =(SELECT current_timestamp());

  

  select COUNT(*),memberID into _usercount,_memberID from tblmember where 

    tblmember.socialType = _socialType and 

    tblmember.socialID = _socialID and 

    tblmember.pw = _pw;

    

  SET _authToken=(SELECT uuid());

        

  if _usercount>0 then

    insert into tblloginstate (memberid,authtoken,lastlogintime) VALUES(_memberID,_authToken,_currentTime) ON DUPLICATE KEY UPDATE authtoken=_authToken,lastlogintime=_currentTime;

    select _authToken;

  else

    select -1;

  end if;  

 

  

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_validtoken` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_validtoken`(

  IN _authtoken VARCHAR(45)

  )
BEGIN  

  DECLARE usercount INT;  

  DECLARE _memberid INT;

  

  select COUNT(*),memberid into usercount,_memberid from tblloginstate;  

  if usercount >0 then

    select memberID,socialType,socialID,nick,picurl,email,createtime from tblmember where memberID = _memberid;

  else

    select -1;

  end if;  

  

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `uv_tblmember`
--

/*!50001 DROP VIEW IF EXISTS `uv_tblmember`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `uv_tblmember` AS select `tblmember`.`nick` AS `nick`,`tblmember`.`picurl` AS `picurl` from `tblmember` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-11-20  2:18:07
