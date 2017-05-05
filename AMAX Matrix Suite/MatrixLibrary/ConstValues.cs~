//
//  Copyright 2017  AMAX Information Technologies, Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Collections.Generic;

namespace MatrixLibrary
{
	public static class ConstValues
	{
		// Message Strings
		//Error Messages
		static public String MSG_ERR_NO_LICENSE_FOUND = @"No Validated License Found!\N Please contact AMAX Sales!";
		static public String MSG_ERR_LOAD_SERVER_CONFIG_FAILED = @"Load Server Configuration File Failed!";
		static public String MSG_ERR_LOAD_AGENT_CONFIG_FAILED = @"Load Agent Configuration File Failed!";
		static public String MSG_ERR_INVALID_CONFIG_FILE = @"Configuration File Path is invalid!";
		static public String MSG_ERR_TOO_MANY_ARGUMENTS = @"Too many arguments!";
		static public String MSG_ERR_DB_STRUCTURE_NOT_MATCH = @"Required Database Structure Version doesn't match!";
		static public String MSG_ERR_SERVICE_ID_NOT_MATCH = @"Matrix Service ID doesn't match the value in Database!";
		static public String MSG_ERR_SERVICE_INFO_VALIDATION_FAILD = @"Service Information Validation failed!";
		static public String MSG_ERR_DATABASE_INIT_FAILED = @"Database initializing failed!";
		static public String MSG_ERR_SENDCMD_FAILD = @"Send Command to Server Deamon failed! Message: {0}. Command:{1}";
		static public String MSG_ERR_RECVCMD_FAILD = @"Receive Command from Client failed! Message: {0}.";
		static public String MSG_ERR_DB_ACCESS_DENY = @"Database Access Denied! Please check the database login information then try again!";
		static public String MSG_ERR_DB_NOT_EXISTED = @"Database Not Exists!";
		static public String MSG_ERR_DB_NOT_INITIALIZED = @"Database Not Initialized";
		static public String MSG_ERR_SERVER_SOCKET_INIT_FAILED = @"Matrix Server Initialize Failed!";
		static public String MSG_ERR_AGENT_SOCKET_INIT_FAILED = @"Matrix Agent Initialize Failed!";
		static public String MSG_ERR_FAILED_PARSE_AGENT_CONFIG = @"Failed to parse the Agent configuration file!";
		static public String MSG_ERR_AGENT_AUTH_CHECK_FAILED = @"This agent is not registed or deactived. Please contact administrator for help!";
		static public String MSG_ERR_AGENT_REGISTRATION_REQUEST_FAILED = @"Failed to send the registration request! You can restart the agent and it will send the request again!";
		static public String MSG_ERR_SYSTEM_CALL_FAILED = @"System Call Failed. CMD: {0} Parameters: {1}";
		static public String MSG_ERR_CUDA_PATH_CHECK_FAILED = @"The path of CUDA doesn't existing! Path: {0} \n If the CUDA is not installed under default path, please change the path in the agent.conf.";
		static public String MSG_ERR_NVIDIA_SMI_PATH_CHECK_FAILED = @"The path of 'nvidia-smi' doesn't existing! Path: {0} \n If the CUDA is not installed under default path, please change the path in the agent.conf.";


		// Warning Message
		static public String MSG_WARN_INVILID_CMDSTRING = @"The command string is invalid! CmdString: {0}";
		static public String MSG_WARN_INVALID_HASH_CMDSTRING=@"The Hash of the command string is invalid! Expected:{0} Actually:{1}";
		static public String MSG_WARN_AGENT_NOT_REGISTED = @"This Agent({0}) was not registered by Resource Manager!";
		static public String MSG_WARN_AGENT_NOT_APPROVED = @"This Agent({0}) was not approved by administrator or Resource!";
		static public String MSG_WARN_INVALID_RESPONSE_STRING = @"The response content is invalid! Response: {0}";
		static public String MSG_WARN_INVALID_HASH_RESPONSE=@"The Hash of the response string is invalid! Expected:{0} Actually:{1}";


		// Info Messages
		static public String MSG_INFO_INIT_MATRIX_UUID = @"Initial Service ID for Matrix Server";
		static public String MSG_INFO_MATRIX_SRV_LISTENING = @"Matrix Server is listening on {0}:{1} ";
		static public String MSG_INFO_MATRIX_AGENT_LISTENING = @"Matrix Agent is listening on {0}:{1} ";
		static public String MSG_INFO_CONNECTION_ESTABLISHED = @"Connected with {0}";
		static public String MSG_INFO_RECEIVED_STRING = @"Received String: {0}";
		static public String MSG_INFO_RESPONSE_STRING_BUILDING = @"Response String: {0}";
		static public String MSG_INFO_AGENT_SEND_REGIST_REQUEST = @"Agent has send the registation requesation to Sever! It shoudl be in pending status and waiting for administator's approval!";


		// Debug Message
		static public String MSG_DBG_CMD_RECEIVED = @"Received Command String: {0}";
		static public String MSG_DBG_FAKE_RESPONSE_STRING = @"17:This is a fake Response";
		static public String MSG_DBG_CMD_SEND =@"Command has been Send out! Command: {0}";
		static public String MSG_DBG_RESPONSE_SEND = @"Response String has been send out! Response: {0}";
		static public String MSG_DBG_ENCODING_BASE64_STRING = @"Original String: {0} \nEncoded Base64 String: {1}";
		static public String MSG_DBG_DECODING_BASE64_STRING = @"Base64 String: {0} \nDecoded String: {1}";
		static public String MSG_DBG_CMD_STRING_BUILDING = @"Command String: {0}";


		// Program Const Variables
		static public String APP_REQUIRED_DB_VERSION = @"1.0";
		static public String APP_VERSION = @"0.1";
		static public String DEFAULT_CUDA_FOLDER = @"/usr/local/cuda";
		static public String DEFAULT_PATH_NVIDIA_SMI = @"/usr/bin/nvidia-smi";
		static public String DEFAULT_SERVER_CONFIG_PATH = @"/etc/matrix/server.conf";
		static public String DEFAULT_AGENT_CONFIG_PATH = @"/etc/matrix/agent.conf";
		static public String DEFAULT_CLIENT_RUNTIME_CONFIG = @"/opt/amax/matrix/load_client_runtime_config";
		static public String DEFAULT_SERVER_RUNTIME_CONFIG = @"/opt/amax/matrix/load_server_runtime_config";
		static public String DEFAULT_AGENT_DEPLOY_PATH = @"/opt/amax/matrix/";
		static public String DEFAULT_SERVER_DEPLOY_PATH = @"/opt/amax/matrix/";
		static public String DEFAULT_DEPLOY_PATH = @"/opt/amax/matrix";
		static public String DEFAULT_MATRIX_CONFIG_PATH = @"/etc/matrix";
		static public String DEFAULT_MATRIX_LOG_PATH = @"/var/log/matrix";
		static public String DEPLOY_SERVER_APP_SOURCE_PATH = @"./Deploy/Server/./";
		static public String DEPLOY_AGENT_APP_SOURCE_PATH = @"./Deploy/Agent/./";
		static public String DEPLOY_RCUDA_APP_SOURCE_PATH = @"./Deploy/rCuda/";
		static public String RCUDA_RUN_BINARY_WITH_PATH = @"/opt/amax/matrix/rCuda/bin/rCUDAd";
		static public String RCUDA_BIN_PATH = @"/opt/amax/matrix/rCuda/bin";
		static public String RCUDA_LIB_PATH = @"/opt/amax/matrix/rCuda/lib";
		static public String RCUDA_LIB_CUDNN_PATH = @"/opt/amax/matrix/rCuda/lib/cudnn";
		static public String DEPLOY_SQL_INIT_SCRIPT = @"./Deploy/db_matrix.sql";
		static public String SERVER_DAEMON_STARTUP_SCRIPT = @"./Deploy/Server/matrix-server";
		static public String AGENT_DAEMON_STARTUP_SCRIPT = @"./Deploy/Agent/matrix-agent";
		static public String STARTUP_SCRIPT_PATH = @"/etc/init.d/";





		// Return Code
		public const int CODE_NORMAL_EXIT = 0;
		public const int CODE_NO_LICENSE = 100;
		public const int CODE_PARAMETER_NUM_INCORRECT = 101;
		public const int CODE_FAILED_TO_GET_CONFIG = 102;
		public const int CODE_INVALID_CONFIG_FILE_PATH = 103;
		public const int CODE_SERVICE_CHECK_FAILED = 104;
		public const int CODE_DATABASE_INIT_FAILED = 105;
		public const int CODE_SOCKET_INIT_FAILED = 106;
		public const int CODE_INVALID_AGENT_CONFIG_CONTENT = 107;
		public const int CODE_AGENT_AUTH_CHECK_FAILED = 108;

		// Protocol Function Code
		// Client Function format is 1xx
		// Server Function format is 2xx
		// GUI Function format is 3xx
		public const int PROTOCOL_INIT_CODE = 0xffffff;
		public const int PROTOCOL_ACKNOWLEDGEMENT_SUCCESS = 0;
		public const int PROTOCOL_ACKNOWLEDGEMENT_FAILD = 1;   //Retry the package with same sokcet session
		public const int PROTOCOL_ACKNOWLEDGEMENT_FAILD_TERMINATE_SESSION = -1;
		public const int PROTOCOL_FN_GUI_HANDSHAKE_WITH_SERVER = 77;
		public const int PROTOCOL_FN_UNREGISTER_AGENT = 101;
		public const int PROTOCOL_FN_HEARTBEAT_CHECK = 102;
		public const int PROTOCOL_FN_AGENT_AUTHENTICATION_CHECK = 103;
		public const int PROTOCOL_FN_REQUEST_FOR_RUNTIME_CONFIG= 104;
		public const int PROTOCOL_FN_UPLOAD_GPU_INFO = 105;
		public const int PROTOCOL_FN_UPLOAD_FABRIC_INFO = 106;
		public const int PROTOCOL_FN_UPDATE_AGENT_STATUS = 107;
		public const int PROTOCOL_FN_REQUEST_ASSIGNMENT_CONFIG = 108;
		public const int PROTOCOL_FN_REQUEST_RCUDA_SERVER_CONFIG = 109;
		public const int PROTOCOL_FN_PUSH_NEW_ASSIGNMENT = 201;
		public const int PROTOCOL_FN_FORCE_RELOAD_RUNTIME_CONFIG = 202;
		public const int PROTOCOL_FN_SHUTDOWN_RCUDA_CLIENT = 203;
		public const int PROTOCOL_FN_SAVE_RCUDA_SERVER_CONFIG = 301;
		public const int PROTOCOL_FN_REGISTER_AGENT = 302;
		public const int PROTOCOL_FN_GET_AGENT_LIST = 303;
		public const int PROTOCOL_FN_GET_GPU_LIST = 304;
		public const int PROTOCOL_FN_GET_FABRIC_LIST = 305;
		public const int PROTOCOL_FN_GET_AGENT_ASSIGNMENT_LIST = 306;
		public const int PROTOCOL_FN_GET_RCUDA_SRV_CONFIG = 307;
		public const int PROTOCOL_FN_GET_RCUDA_CLIENT_CONFIG = 308;
		public const int PROTOCOL_FN_GET_GPU_VIEW_LIST = 309;
		public const int PROTOCOL_FN_VERIFY_GPU_EXCLUSIVE = 310;
		public const int PROTOCOL_FN_SAVE_ASSIGNMENT = 311;
		public const int PROTOCOL_FN_SAVE_RCUDA_CLIENT_CONFIG = 312;
		public const int PROTOCOL_FN_GET_AGENT_LIST_BY_GPU = 313;
		public const int PROTOCOL_FN_GET_GPU_XML = 314;
		public const int PROTOCOL_FN_RESTART_AGENT = 315;
		public const int PROTOCOL_FN_CHECK_RESOURCE_IF_ASSIGNED = 316;
		public const int PROTOCOL_FN_REMOVE_AGENT = 317;





		// Database Initiate SQLs
		static public String DEFAULT_DB_NAME= @"matrix";
		static public String DEFAULT_TB_GPU_RESOURCE = @"resource";
		static public String DEFAULT_TB_AGENT_INFO = @"agentinfo";
		static public String DEFAULT_TB_RESOURCE_INFO = @"resourceinfo";
		static public String DEFAULT_TB_SERVER_INFO = @"information";
		static public String SQL_CREATE_DATABASE = @"CREATE SCHEMA `matrix` DEFAULT CHARACTER SET utf8" ;
		static public String SQL_CREATE_TABLE_RESOURCE_INFO =@"CREATE TABLE `matrix`.`resourceinfo` ( `id` INT NOT NULL AUTO_INCREMENT, `agent_id` VARCHAR(255) NOT NULL, `gpu_uuid` VARCHAR(128) NOT NULL,  `gpu_id` INT NOT NULL,  `gpu_model` VARCHAR(128) NOT NULL,  `gpu_xml_info` TEXT NULL,  `assign_list` TEXT NULL,  `assign_status` VARCHAR(20) NULL,  `memo` TEXT NULL,  PRIMARY KEY (`id`),  UNIQUE INDEX `gpu_uuid_UNIQUE` (`gpu_uuid` ASC))";
		static public String SQL_CREATE_TABLE_AGENT_INFO = @"CREATE TABLE `matrix`.`agentinfo` (`id` INT NOT NULL AUTO_INCREMENT, `agent_id` VARCHAR(255) NOT NULL,  `agent_type` VARCHAR(20) NOT NULL,  `last_sync_time` VARCHAR(255) NULL,  `uptime` VARCHAR(45) NULL,  `login_user` VARCHAR(255) NULL,  `login_pass` VARCHAR(255) NULL,  `ip_address` TEXT NULL, `agent_port` INT NULL, `agent_status` VARCHAR(20) NULL,  `system_location` VARCHAR(1024) NULL,  `registered_server` VARCHAR(15) NULL,  `memo` TEXT NULL,  PRIMARY KEY (`id`),  UNIQUE INDEX `agent_id_UNIQUE` (`agent_id` ASC))";
		static public String SQL_CREATE_TABLE_SERVER_INFO = @"CREATE TABLE `matrix`.`information` (`index` INT NOT NULL AUTO_INCREMENT,  `key` VARCHAR(255) NOT NULL,  `value` TEXT NULL,  PRIMARY KEY (`index`),  UNIQUE INDEX `key_UNIQUE` (`key` ASC))";
		static public String SQL_CREATE_TABLE_FABRIC_INFO = @"CREATE TABLE `matrix`.`fabricinfo` (`id` INT NOT NULL AUTO_INCREMENT, `agent_id` VARCHAR(255) NOT NULL, `mac_addr` VARCHAR(12) NOT NULL,`ip_addr` TEXT NULL,  `nic_name` VARCHAR(45) NOT NULL, `link_speed` VARCHAR(45) NULL,`op_status` VARCHAR(10) NOT NULL,`gateway_ip` VARCHAR(15) NULL DEFAULT NULL,`is_dhcp` TINYINT NULL DEFAULT NULL,`max_link_speed` VARCHAR(45) NULL DEFAULT NULL,`is_ib` TINYINT NULL DEFAULT NULL,`memo` TEXT NULL DEFAULT NULL,PRIMARY KEY (`id`),UNIQUE INDEX `mac_addr_UNIQUE` (`mac_addr` ASC))";
		static public String SQL_INSERT_TABLE_SERVER_INFO = @"INSERT INTO `matrix`.`information` (`index`,`key`,`value`) VALUES ('1', 'service_id',''),('2', 'service_ip_list',''),('3', 'service_hostname',''),('4', 'db_version','0.1'),('5', 'roles','admin,resadmin,user'),('6', 'last_startup_time','')";

		//Database Query SQLs
		static public String SQL_QUERY_SERVICE_ID = @"SELECT `key`, `value` FROM `information` WHERE `key`='service_id'";
		static public String SQL_QUERY_REQUIRED_DB_VERSION = @"SELECT `value` FROM `information` WHERE `key`='db_version'";
		static public String SQL_QUERY_AGENT_STATUS = @"SELECT `agent_id`,`agent_status` FROM `agentinfo` WHERE `agent_id`='{0}'";

		//Database Insert SQLs
		static public String SQL_INSERT_AGENT_REGISTER = @"INSERT INTO `agentinfo` (`agent_id`, `agent_type`, `ip_address`, `agent_port`, `agent_status`, `registered_server`) VALUES ('{0}','{1}','{2}','{3}','Pending','{4}') ON DUPLICATE KEY UPDATE `agent_type`='{5}', `ip_address`='{6}', `agent_port`='{7}', `agent_status`='Pending', `registered_server`='{8}'";
		static public String SQL_INSERT_GPU_INFO = @"";
		static public String SQL_INSERT_FABRIC_INFO = @"";


		//Database Update SQLs
		static public String SQL_UPDATE_SERVICE_ID = @"UPDATE `information` SET `value`='{0}' WHERE `key`='service_id'";
		static public String SQL_UPDATE_AGENT_STATUS = @"UPDATE `agentinfo` SET `agent_status`='{0}' WHERE `agent_id`='{1}'";

		//Database Delete SQLs


		// System Call Command & Parameters
		static public String PARAMS_NVIDIA_SMI_GET_XML_INFO = @" -q -x | tail -n +3";

	}
}
