CREATE DATABASE  IF NOT EXISTS `matrix` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `matrix`;
-- MySQL dump 10.13  Distrib 5.7.18, for Linux (x86_64)
--
-- Host: 157.22.244.236    Database: matrix
-- ------------------------------------------------------
-- Server version	5.5.55-0ubuntu0.14.04.1

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
-- Table structure for table `agentinfo`
--

DROP TABLE IF EXISTS `agentinfo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `agentinfo` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `agent_id` varchar(255) NOT NULL,
  `agent_type` varchar(20) NOT NULL,
  `last_sync_time` varchar(255) DEFAULT NULL,
  `uptime` varchar(45) DEFAULT NULL,
  `login_user` varchar(255) DEFAULT NULL,
  `login_pass` varchar(255) DEFAULT NULL,
  `agent_ip` text,
  `agent_port` int(11) DEFAULT NULL,
  `agent_status` varchar(20) DEFAULT NULL,
  `system_location` varchar(1024) DEFAULT NULL,
  `registered_server_ip` varchar(15) DEFAULT NULL,
  `registered_server_port` int(11) DEFAULT NULL,
  `memo` text,
  PRIMARY KEY (`id`),
  UNIQUE KEY `agent_id_UNIQUE` (`agent_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `assignment`
--

DROP TABLE IF EXISTS `assignment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `assignment` (
  `agent_id` varchar(255) NOT NULL,
  `gpu_uuid` varchar(128) NOT NULL,
  `resource_ip` varchar(15) NOT NULL,
  PRIMARY KEY (`agent_id`,`gpu_uuid`,`resource_ip`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fabricinfo`
--

DROP TABLE IF EXISTS `fabricinfo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fabricinfo` (
  `mac_addr` varchar(12) NOT NULL,
  `agent_id` varchar(255) NOT NULL,
  `ip_addr` text,
  `nic_name` varchar(45) NOT NULL,
  `link_speed` varchar(45) DEFAULT NULL,
  `op_status` varchar(10) NOT NULL,
  `gateway_ip` varchar(15) DEFAULT NULL,
  `is_dhcp` tinyint(4) DEFAULT NULL,
  `max_link_speed` varchar(45) DEFAULT NULL,
  `is_ib` tinyint(4) DEFAULT NULL,
  `memo` text,
  PRIMARY KEY (`mac_addr`),
  UNIQUE KEY `mac_addr_UNIQUE` (`mac_addr`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `information`
--

DROP TABLE IF EXISTS `information`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `information` (
  `index` int(11) NOT NULL AUTO_INCREMENT,
  `key` varchar(255) NOT NULL,
  `value` text,
  PRIMARY KEY (`index`),
  UNIQUE KEY `key_UNIQUE` (`key`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `resourceinfo`
--

DROP TABLE IF EXISTS `resourceinfo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `resourceinfo` (
  `gpu_uuid` varchar(128) NOT NULL,
  `agent_id` varchar(255) NOT NULL,
  `gpu_id` int(11) NOT NULL,
  `gpu_model` varchar(128) NOT NULL,
  `gpu_xml_info` text,
  `assign_type` varchar(20) DEFAULT NULL,
  `memo` text,
  PRIMARY KEY (`gpu_uuid`),
  UNIQUE KEY `gpu_uuid_UNIQUE` (`gpu_uuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `runtimeconf`
--

DROP TABLE IF EXISTS `runtimeconf`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `runtimeconf` (
  `agent_id` varchar(255) NOT NULL,
  `rcuda_server_conf` text,
  `rcuda_client_conf` text,
  PRIMARY KEY (`agent_id`),
  UNIQUE KEY `agent_id_UNIQUE` (`agent_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping events for database 'matrix'
--

--
-- Dumping routines for database 'matrix'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-05-04 10:17:23
