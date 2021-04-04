/*
 Navicat Premium Data Transfer

 Source Server         : 119.27.173.241postgresql
 Source Server Type    : PostgreSQL
 Source Server Version : 120002
 Source Host           : 119.27.173.241:5432
 Source Catalog        : haohaoPlay
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 120002
 File Encoding         : 65001

 Date: 04/04/2021 21:24:47
*/


-- ----------------------------
-- Table structure for sysdict
-- ----------------------------
DROP TABLE IF EXISTS "public"."sysdict";
CREATE TABLE "public"."sysdict" (
  "id" int8 NOT NULL,
  "dictcode" varchar(64) COLLATE "pg_catalog"."default",
  "dictname" varchar(64) COLLATE "pg_catalog"."default",
  "parentid" int8,
  "itemname" varchar(64) COLLATE "pg_catalog"."default",
  "sort" int4,
  "creatorid" int8,
  "createtime" timestamp(0),
  "modifierid" int8,
  "modifytime" timestamp(0),
  "isdeleted" bool DEFAULT false,
  "itemvalue" int4,
  "remark" varchar(255) COLLATE "pg_catalog"."default",
  "dicttype" int4
)
;
ALTER TABLE "public"."sysdict" OWNER TO "postgres";
COMMENT ON COLUMN "public"."sysdict"."dictcode" IS '编码';
COMMENT ON COLUMN "public"."sysdict"."dictname" IS '名称';
COMMENT ON COLUMN "public"."sysdict"."parentid" IS '父级id';
COMMENT ON COLUMN "public"."sysdict"."itemname" IS '数据项名称';
COMMENT ON COLUMN "public"."sysdict"."sort" IS '排序值';
COMMENT ON COLUMN "public"."sysdict"."itemvalue" IS '数据项值';
COMMENT ON COLUMN "public"."sysdict"."remark" IS '备注信息';
COMMENT ON COLUMN "public"."sysdict"."dicttype" IS '字典类型';

-- ----------------------------
-- Records of sysdict
-- ----------------------------
BEGIN;
INSERT INTO "public"."sysdict" VALUES (1377799905854103552, 'MoneyType', '费用类型', -1, NULL, 0, -1, '2021-04-02 09:47:51', NULL, NULL, 'f', NULL, NULL, 0);
INSERT INTO "public"."sysdict" VALUES (1377803743998775296, 'CarType', '车辆类型', 1377803522313031680, '轿车', 1, -1, '2021-04-02 10:03:06', NULL, NULL, 'f', 1, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377857079624077312, 'OrderStatus', '订单状态', -1, NULL, 0, -1, '2021-04-02 13:35:02', NULL, NULL, 'f', NULL, NULL, 0);
INSERT INTO "public"."sysdict" VALUES (1377857368448045056, 'OrderStatus', '订单状态', 1377857079624077312, '货已送达', 3, -1, '2021-04-02 13:36:11', NULL, NULL, 'f', 3, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377857870812418048, 'SettleStatus', '结算状态', -1, NULL, 0, -1, '2021-04-02 13:38:10', NULL, NULL, 'f', NULL, NULL, 0);
INSERT INTO "public"."sysdict" VALUES (1377857897685323776, 'SettleStatus', '结算状态', 1377857870812418048, '无结算单据', 1, -1, '2021-04-02 13:38:17', NULL, NULL, 'f', 1, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377857915595001856, 'SettleStatus', '结算状态', 1377857870812418048, '结算单据未回', 2, -1, '2021-04-02 13:38:21', NULL, NULL, 'f', 2, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377857933680840704, 'SettleStatus', '结算状态', 1377857870812418048, '结算单据已回', 3, -1, '2021-04-02 13:38:25', NULL, NULL, 'f', 3, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377858427753074688, 'CarriageType', '承载类别', -1, NULL, 0, -1, '2021-04-02 13:40:23', NULL, NULL, 'f', NULL, NULL, 0);
INSERT INTO "public"."sysdict" VALUES (1377858767449755648, 'SendVehicleType', '派车方式', -1, NULL, 0, -1, '2021-04-02 13:41:44', NULL, NULL, 'f', NULL, NULL, 0);
INSERT INTO "public"."sysdict" VALUES (1377872400024080384, 'ReceiptStatus', '回单状态', 1377858882910556160, '未回', 1, -1, '2021-04-02 14:35:54', NULL, NULL, 'f', 1, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377872454571003904, 'ReceiptStatus', '回单状态', 1377858882910556160, '已回', 2, -1, '2021-04-02 14:36:07', NULL, NULL, 'f', 2, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1378311159026814976, 'SendVehicleType', '派车方式', 1377858767449755648, '方式1', 1, -1, '2021-04-03 19:39:23', NULL, NULL, 'f', 1, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1378311180027695104, 'SendVehicleType', '派车方式', 1377858767449755648, '方式2', 2, -1, '2021-04-03 19:39:28', NULL, NULL, 'f', 2, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1378311226248925184, 'FeeStatus', '费用状态', 1377858625380290560, '状态1', 1, -1, '2021-04-03 19:39:39', NULL, NULL, 'f', 1, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1378311251645435904, 'FeeStatus', '费用状态', 1377858625380290560, '状态2', 2, -1, '2021-04-03 19:39:45', NULL, NULL, 'f', 2, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1378311301431824384, 'ContractType', '合同类型', 1377858574914424832, '类型1', 1, -1, '2021-04-03 19:39:57', NULL, NULL, 'f', 1, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1378311337993572352, 'ContractType', '合同类型', 1377858574914424832, '类型2', 2, -1, '2021-04-03 19:40:05', NULL, NULL, 'f', 2, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377803522313031680, 'CarType', '车辆类型', -1, NULL, 0, -1, '2021-04-02 10:02:13', NULL, NULL, 'f', NULL, NULL, 0);
INSERT INTO "public"."sysdict" VALUES (1377803581909897216, 'MoneyType', '费用类型', 1377799905854103552, '人民币', 1, -1, '2021-04-02 10:02:27', NULL, NULL, 'f', 1, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377803613337817088, 'MoneyType', '费用类型', 1377799905854103552, '美元', 2, -1, '2021-04-02 10:02:34', NULL, NULL, 'f', 2, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377803653343088640, 'MoneyType', '费用类型', 1377799905854103552, '英镑', 3, -1, '2021-04-02 10:02:44', NULL, NULL, 'f', 3, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377803786566766592, 'CarType', '车辆类型', 1377803522313031680, '货车', 2, -1, '2021-04-02 10:03:16', NULL, NULL, 'f', 2, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377803810461716480, 'CarType', '车辆类型', 1377803522313031680, '火车', 3, -1, '2021-04-02 10:03:21', NULL, NULL, 'f', 3, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377803835245858816, 'CarType', '车辆类型', 1377803522313031680, '三轮车', 4, -1, '2021-04-02 10:03:27', NULL, NULL, 'f', 4, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377857166869794816, '1', '待发货', -1, NULL, 0, -1, '2021-04-02 13:35:23', NULL, NULL, 't', NULL, NULL, 0);
INSERT INTO "public"."sysdict" VALUES (1377857246016311296, 'OrderStatus', '订单状态', 1377857079624077312, '待发货', 1, -1, '2021-04-02 13:35:41', NULL, NULL, 'f', 1, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377857281676283904, 'OrderStatus', '订单状态', 1377857079624077312, '已发货', 2, -1, '2021-04-02 13:35:50', NULL, NULL, 'f', 2, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377857608722944000, 'PhoneSystem', '手机系统', -1, NULL, 0, -1, '2021-04-02 13:37:08', NULL, NULL, 'f', NULL, NULL, 0);
INSERT INTO "public"."sysdict" VALUES (1377857654784790528, 'PhoneSystem', '手机系统', 1377857608722944000, 'IOS', 1, -1, '2021-04-02 13:37:19', NULL, NULL, 'f', 1, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377857687424864256, 'PhoneSystem', '手机系统', 1377857608722944000, 'Android', 2, -1, '2021-04-02 13:37:27', NULL, NULL, 'f', 2, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377857724582203392, 'PhoneSystem', '手机系统', 1377857608722944000, '塞班', 3, -1, '2021-04-02 13:37:36', NULL, NULL, 'f', 3, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1377858298950193152, 'ClientType', '客户类型', -1, NULL, 0, -1, '2021-04-02 13:39:52', NULL, NULL, 'f', NULL, NULL, 0);
INSERT INTO "public"."sysdict" VALUES (1377858574914424832, 'ContractType', '合同类型', -1, NULL, 0, -1, '2021-04-02 13:40:58', NULL, NULL, 'f', NULL, NULL, 0);
INSERT INTO "public"."sysdict" VALUES (1377858625380290560, 'FeeStatus', '费用状态', -1, NULL, 0, -1, '2021-04-02 13:41:10', NULL, NULL, 'f', NULL, NULL, 0);
INSERT INTO "public"."sysdict" VALUES (1377858882910556160, 'ReceiptStatus', '回单状态', -1, NULL, 0, -1, '2021-04-02 13:42:12', NULL, NULL, 'f', NULL, NULL, 0);
INSERT INTO "public"."sysdict" VALUES (1378310930076536832, 'ClientType', '客户类型', 1377858298950193152, '普通客户', 1, -1, '2021-04-03 19:38:28', NULL, NULL, 'f', 1, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1378310960896282624, 'ClientType', '客户类型', 1377858298950193152, 'VIP客户', 2, -1, '2021-04-03 19:38:36', NULL, NULL, 'f', 2, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1378311029955497984, 'CarriageType', '承载类别', 1377858427753074688, '整车', 1, -1, '2021-04-03 19:38:52', NULL, NULL, 'f', 1, NULL, 1);
INSERT INTO "public"."sysdict" VALUES (1378311055314259968, 'CarriageType', '承载类别', 1377858427753074688, '零担', 2, -1, '2021-04-03 19:38:58', NULL, NULL, 'f', 2, NULL, 1);
COMMIT;

-- ----------------------------
-- Table structure for sysloginrecord
-- ----------------------------
DROP TABLE IF EXISTS "public"."sysloginrecord";
CREATE TABLE "public"."sysloginrecord" (
  "id" int8 NOT NULL,
  "userid" int8,
  "ip" varchar(20) COLLATE "pg_catalog"."default",
  "time" timestamp(0),
  "jwtjti" char(36) COLLATE "pg_catalog"."default",
  "jwtexpiretime" timestamp(0)
)
;
ALTER TABLE "public"."sysloginrecord" OWNER TO "postgres";

-- ----------------------------
-- Records of sysloginrecord
-- ----------------------------
BEGIN;
INSERT INTO "public"."sysloginrecord" VALUES (1378699021660459008, -1, '117.90.246.241', '2021-04-04 21:20:36', 'b7404acb-b69d-4c24-a0ce-cdf5df1aa714', '2021-04-07 21:20:36');
INSERT INTO "public"."sysloginrecord" VALUES (1378350333331247104, -1, '117.90.246.241', '2021-04-03 22:15:03', 'c74bd2b9-8944-4cfb-bf72-0bd539d811b5', '2021-04-06 22:15:03');
COMMIT;

-- ----------------------------
-- Table structure for sysmodule
-- ----------------------------
DROP TABLE IF EXISTS "public"."sysmodule";
CREATE TABLE "public"."sysmodule" (
  "id" int8 NOT NULL,
  "name" varchar(20) COLLATE "pg_catalog"."default",
  "icon" varchar(64) COLLATE "pg_catalog"."default",
  "routerurl" varchar(255) COLLATE "pg_catalog"."default",
  "parentid" int8,
  "type" int4,
  "sort" int4 DEFAULT 0,
  "layer" int4,
  "number" int8,
  "creatorid" int8,
  "createtime" timestamp(0),
  "modifierid" int8,
  "modifytime" timestamp(0),
  "isdeleted" bool DEFAULT false,
  "alias" varchar(64) COLLATE "pg_catalog"."default",
  "parentalias" varchar(255) COLLATE "pg_catalog"."default"
)
;
ALTER TABLE "public"."sysmodule" OWNER TO "postgres";
COMMENT ON COLUMN "public"."sysmodule"."name" IS '模块名称';
COMMENT ON COLUMN "public"."sysmodule"."icon" IS '模块图标';
COMMENT ON COLUMN "public"."sysmodule"."routerurl" IS '子应用路由地址';
COMMENT ON COLUMN "public"."sysmodule"."parentid" IS '父级id';
COMMENT ON COLUMN "public"."sysmodule"."type" IS '0:系统 1:主菜单 2:子菜单 3:资源';
COMMENT ON COLUMN "public"."sysmodule"."sort" IS '排序';
COMMENT ON COLUMN "public"."sysmodule"."layer" IS '层级 一层31个 0~30 ';
COMMENT ON COLUMN "public"."sysmodule"."number" IS '权限数字';
COMMENT ON COLUMN "public"."sysmodule"."alias" IS '别名';
COMMENT ON COLUMN "public"."sysmodule"."parentalias" IS '父级别名';

-- ----------------------------
-- Records of sysmodule
-- ----------------------------
BEGIN;
INSERT INTO "public"."sysmodule" VALUES (0, '系统', NULL, NULL, -1, 0, 0, 1, 1, NULL, NULL, NULL, NULL, 'f', 'System', NULL);
INSERT INTO "public"."sysmodule" VALUES (1, '设置', 'icon:xitongpeizhi', '', 0, 1, 100, 1, 2, NULL, NULL, -1, '2020-11-23 19:16:55', 'f', 'Setting', 'System');
INSERT INTO "public"."sysmodule" VALUES (2, '用户管理', NULL, 'setting/user', 1, 2, 1, 1, 4, NULL, NULL, -1, '2021-01-09 22:15:09', 'f', 'User', 'Setting');
INSERT INTO "public"."sysmodule" VALUES (3, '应用管理', NULL, 'setting/app', 1, 2, 100, 1, 8, NULL, NULL, -1, '2020-08-12 14:01:56', 'f', 'App', 'Setting');
INSERT INTO "public"."sysmodule" VALUES (4, '角色管理', NULL, 'setting/role', 1, 2, 121, 1, 16, NULL, NULL, -1, '2020-05-16 09:30:08', 'f', 'Role', 'Setting');
INSERT INTO "public"."sysmodule" VALUES (1261466651195478016, '字典管理', NULL, 'setting/dict', 1, 2, 155, 1, 32, -1, '2020-05-16 09:21:01', -1, '2020-05-21 09:36:27', 'f', 'Dict', 'Setting');
INSERT INTO "public"."sysmodule" VALUES (1353339649711542272, '查询', NULL, NULL, 2, 3, 0, 1, 64, -1, '2021-01-24 21:51:31', -1, '2021-01-24 22:53:12', 'f', 'Search', 'User');
INSERT INTO "public"."sysmodule" VALUES (1353340523598974976, '添加', NULL, NULL, 2, 3, 0, 1, 128, -1, '2021-01-24 21:54:59', NULL, NULL, 'f', 'Add', 'User');
INSERT INTO "public"."sysmodule" VALUES (1353340580607954944, '编辑', NULL, NULL, 2, 3, 0, 1, 256, -1, '2021-01-24 21:55:13', NULL, NULL, 'f', 'Update', 'User');
INSERT INTO "public"."sysmodule" VALUES (1353340631862349824, '删除', NULL, NULL, 2, 3, 0, 1, 512, -1, '2021-01-24 21:55:25', NULL, NULL, 'f', 'Delete', 'User');
INSERT INTO "public"."sysmodule" VALUES (1353340696077144064, '查看', NULL, NULL, 2, 3, 0, 1, 1024, -1, '2021-01-24 21:55:40', NULL, NULL, 'f', 'View', 'User');
INSERT INTO "public"."sysmodule" VALUES (1353340849806774272, '导出', NULL, NULL, 2, 3, 0, 1, 2048, -1, '2021-01-24 21:56:17', NULL, NULL, 'f', 'Export', 'User');
INSERT INTO "public"."sysmodule" VALUES (1353340899110817792, '导入', NULL, NULL, 2, 3, 0, 1, 4096, -1, '2021-01-24 21:56:29', NULL, NULL, 'f', 'Import', 'User');
INSERT INTO "public"."sysmodule" VALUES (1353341063380733952, '查询', NULL, NULL, 3, 3, 0, 1, 8192, -1, '2021-01-24 21:57:08', NULL, NULL, 'f', 'Search', 'App');
INSERT INTO "public"."sysmodule" VALUES (1353341112122740736, '添加', NULL, NULL, 3, 3, 0, 1, 16384, -1, '2021-01-24 21:57:20', NULL, NULL, 'f', 'Add', 'App');
INSERT INTO "public"."sysmodule" VALUES (1353341168343191552, '更新', NULL, NULL, 3, 3, 0, 1, 32768, -1, '2021-01-24 21:57:33', NULL, NULL, 'f', 'Update', 'App');
INSERT INTO "public"."sysmodule" VALUES (1353341283434893312, '删除', NULL, NULL, 3, 3, 0, 1, 65536, -1, '2021-01-24 21:58:00', NULL, NULL, 'f', 'Delete', 'App');
INSERT INTO "public"."sysmodule" VALUES (1353341512045432832, '查询', NULL, NULL, 4, 3, 0, 1, 131072, -1, '2021-01-24 21:58:55', NULL, NULL, 'f', 'Search', 'Role');
INSERT INTO "public"."sysmodule" VALUES (1353341592932585472, '查看权限', NULL, NULL, 4, 3, 0, 1, 262144, -1, '2021-01-24 21:59:14', NULL, NULL, 'f', 'ViewAuth', 'Role');
INSERT INTO "public"."sysmodule" VALUES (1353341657092853760, '更新权限', NULL, NULL, 4, 3, 0, 1, 524288, -1, '2021-01-24 21:59:30', NULL, NULL, 'f', 'UpdateAuth', 'Role');
INSERT INTO "public"."sysmodule" VALUES (1353341730488979456, '查询', NULL, NULL, 1261466651195478016, 3, 0, 1, 1048576, -1, '2021-01-24 21:59:47', NULL, NULL, 'f', 'Search', 'Dict');
INSERT INTO "public"."sysmodule" VALUES (1353341771932897280, '添加', NULL, NULL, 1261466651195478016, 3, 0, 1, 2097152, -1, '2021-01-24 21:59:57', NULL, NULL, 'f', 'Add', 'Dict');
INSERT INTO "public"."sysmodule" VALUES (1353341816891641856, '编辑', NULL, NULL, 1261466651195478016, 3, 0, 1, 4194304, -1, '2021-01-24 22:00:08', -1, '2021-01-24 22:00:39', 'f', 'Edit', 'Dict');
INSERT INTO "public"."sysmodule" VALUES (1353341868557078528, '删除', NULL, NULL, 1261466651195478016, 3, 0, 1, 8388608, -1, '2021-01-24 22:00:20', NULL, NULL, 'f', 'Delete', 'Dict');
INSERT INTO "public"."sysmodule" VALUES (1353344073079066624, '注销', NULL, NULL, 2, 3, 0, 1, 16777216, -1, '2021-01-24 22:09:06', NULL, NULL, 'f', 'Disable', 'User');
INSERT INTO "public"."sysmodule" VALUES (1353344144717778944, '启用', NULL, NULL, 2, 3, 0, 1, 33554432, -1, '2021-01-24 22:09:23', NULL, NULL, 'f', 'Enable', 'User');
COMMIT;

-- ----------------------------
-- Table structure for sysrole
-- ----------------------------
DROP TABLE IF EXISTS "public"."sysrole";
CREATE TABLE "public"."sysrole" (
  "id" int8 NOT NULL,
  "creatorid" int8,
  "createtime" timestamp(0),
  "modifierid" int8,
  "modifytime" timestamp(0),
  "isdeleted" bool DEFAULT false,
  "name" varchar(255) COLLATE "pg_catalog"."default",
  "authnumbers" varchar(255) COLLATE "pg_catalog"."default",
  "level" int4
)
;
ALTER TABLE "public"."sysrole" OWNER TO "postgres";
COMMENT ON COLUMN "public"."sysrole"."name" IS '角色名称';

-- ----------------------------
-- Records of sysrole
-- ----------------------------
BEGIN;
INSERT INTO "public"."sysrole" VALUES (1, -1, NULL, -1, '2021-01-24 22:25:47', 'f', '超级管理员', '[67108863]', 0);
INSERT INTO "public"."sysrole" VALUES (2, -1, NULL, -1, '2021-01-24 22:40:03', 'f', '管理员', '[67108863]', 1);
INSERT INTO "public"."sysrole" VALUES (3, -1, NULL, -1, '2021-03-05 14:18:47', 'f', '普通用户', '[7864371]', 2);
COMMIT;

-- ----------------------------
-- Table structure for sysuser
-- ----------------------------
DROP TABLE IF EXISTS "public"."sysuser";
CREATE TABLE "public"."sysuser" (
  "id" int8 NOT NULL,
  "account" varchar(64) COLLATE "pg_catalog"."default",
  "password" varchar(64) COLLATE "pg_catalog"."default",
  "name" varchar(64) COLLATE "pg_catalog"."default",
  "gender" int4,
  "phone" varchar(64) COLLATE "pg_catalog"."default",
  "email" varchar(64) COLLATE "pg_catalog"."default",
  "qq" varchar(64) COLLATE "pg_catalog"."default",
  "wechat" varchar(64) COLLATE "pg_catalog"."default",
  "enabled" bool,
  "firstnameinitial" varchar(64) COLLATE "pg_catalog"."default",
  "lastlogintime" timestamp(0),
  "lastloginip" varchar(64) COLLATE "pg_catalog"."default",
  "creatorid" int8,
  "createtime" timestamp(0),
  "modifierid" int8,
  "modifytime" timestamp(0),
  "isdeleted" bool DEFAULT false,
  "headimgurl" varchar(255) COLLATE "pg_catalog"."default",
  "profile" varchar(255) COLLATE "pg_catalog"."default",
  "homeaddress" varchar(255) COLLATE "pg_catalog"."default",
  "passwordlevel" int4,
  "roleid" int8,
  "authnumbers" varchar(255) COLLATE "pg_catalog"."default",
  "rolename" varchar(20) COLLATE "pg_catalog"."default",
  "rolelevel" int4,
  "birthday" timestamp(0)
)
;
ALTER TABLE "public"."sysuser" OWNER TO "postgres";
COMMENT ON COLUMN "public"."sysuser"."account" IS '账号';
COMMENT ON COLUMN "public"."sysuser"."password" IS '密码';
COMMENT ON COLUMN "public"."sysuser"."profile" IS '个人简介';
COMMENT ON COLUMN "public"."sysuser"."homeaddress" IS '地址';
COMMENT ON COLUMN "public"."sysuser"."passwordlevel" IS '密码强度等级';
COMMENT ON COLUMN "public"."sysuser"."roleid" IS '角色id';
COMMENT ON COLUMN "public"."sysuser"."authnumbers" IS '拥有权限';
COMMENT ON COLUMN "public"."sysuser"."rolename" IS '角色名称';
COMMENT ON COLUMN "public"."sysuser"."rolelevel" IS '角色等级';
COMMENT ON COLUMN "public"."sysuser"."birthday" IS '出生日期';

-- ----------------------------
-- Records of sysuser
-- ----------------------------
BEGIN;
INSERT INTO "public"."sysuser" VALUES (1, 'rongguohao', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '戎国浩', 1, '18168586750', '843468011@qq.com', NULL, 'rongguohao', 'f', 'X', '2021-01-09 16:27:00', '127.0.0.1', -1, '2020-02-16 17:20:08', -1, '2021-04-03 19:45:06', 'f', 'https://haohaoplay-1253596932.cos.ap-nanjing.myqcloud.com/avatar/1_1605018514', 'stay foolish', '镇江京口区', 0, 2, '[67108863]', '管理员', 1, '1992-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (2, 'guest', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', 'Guest', 1, '13073371506', NULL, NULL, NULL, 't', 'G', '2021-03-29 14:44:30', '221.230.31.129', -1, '2020-12-07 13:21:37', NULL, NULL, 'f', NULL, NULL, NULL, 0, 2, '[67108863]', '管理员', 1, '1992-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1306406941723070464, 'libai', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '李白', 1, '18168586750', NULL, NULL, NULL, 't', 'L', '2020-09-18 20:51:32', '127.0.0.1', -1, '2020-09-17 09:37:41', -1, '2021-04-02 13:44:11', 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1992-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1335821400094674944, 'direnjie', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '狄仁杰', 1, '18168586750', NULL, NULL, NULL, 't', 'D', NULL, NULL, -1, '2020-12-07 13:40:15', NULL, NULL, 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1972-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1335821550036848640, 'caiwenji', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '蔡文姬', 0, '18168586750', NULL, NULL, NULL, 't', 'C', '2021-01-18 16:08:07', '221.230.31.129', -1, '2020-12-07 13:40:51', -1, '2021-02-03 23:08:47', 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1982-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1335821729636945920, 'gaojianli', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '高渐离', 1, '18168586750', NULL, NULL, NULL, 't', 'G', '2021-01-18 16:08:51', '221.230.31.129', -1, '2020-12-07 13:41:33', NULL, NULL, 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1992-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1335821883890864128, 'caocao', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '曹操', 1, '18168586750', '232@qq.com', NULL, NULL, 't', 'C', NULL, NULL, -1, '2020-12-07 13:42:10', NULL, NULL, 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1994-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1335821998726713344, 'luna', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '露娜', 0, '18168586750', NULL, NULL, NULL, 't', 'L', NULL, NULL, -1, '2020-12-07 13:42:38', NULL, NULL, 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1994-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1335822168281452544, 'jiangziya', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '姜子牙', 1, '18168586750', NULL, NULL, NULL, 't', 'J', '2021-01-18 16:08:36', '221.230.31.129', -1, '2020-12-07 13:43:18', -1, '2021-01-04 16:48:06', 't', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1996-01-12 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1335822269536145408, 'zhugeliang', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '诸葛亮', 1, '18168586750', NULL, NULL, NULL, 't', 'Z', NULL, NULL, -1, '2020-12-07 13:43:42', NULL, NULL, 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1995-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1335822386452369408, 'anqila', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '安其拉', 0, '18168586750', NULL, NULL, NULL, 'f', 'A', '2020-12-09 21:59:29', '127.0.0.1', -1, '2020-12-07 13:44:10', -1, '2021-01-20 23:14:19', 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1992-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1335822554115477504, 'chengyaojin', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '程咬金', 1, '18168586750', NULL, NULL, NULL, 't', 'C', NULL, NULL, -1, '2020-12-07 13:44:50', -1, '2020-12-07 13:45:08', 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1993-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1335822737884712960, 'zhaoyun', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '赵云', 1, '18168586750', NULL, NULL, NULL, 't', 'Z', NULL, NULL, -1, '2020-12-07 13:45:34', NULL, NULL, 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1990-01-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1335822859540500480, 'lanlinwang', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '兰陵王', 1, '18168586750', NULL, NULL, NULL, 't', 'L', '2021-01-18 16:07:41', '221.230.31.129', -1, '2020-12-07 13:46:03', NULL, NULL, 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1984-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1335822995234623488, 'zhangfei', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '张飞', 1, '18168586750', NULL, NULL, NULL, 't', 'Z', NULL, NULL, -1, '2020-12-07 13:46:35', NULL, NULL, 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1988-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1338713568622809088, 'makeboluo', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '马可波罗', 1, '18168586750', '843468011@qq.com', NULL, NULL, 't', 'M', NULL, NULL, -1, '2020-12-15 13:12:42', NULL, NULL, 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1982-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1338714370275938304, 'hanxin', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '韩信', 1, '18168586750', '84123@qq.com', '12345623', NULL, 't', 'H', '2021-01-18 16:07:05', '221.230.31.129', -1, '2020-12-15 13:15:53', -1, '2021-01-04 14:56:07', 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1952-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1347821880492560384, 'yangguifei', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '杨贵妃', 0, '18168586750', NULL, NULL, NULL, 'f', 'Y', NULL, NULL, -1, '2021-01-09 16:25:52', -1, '2021-04-03 21:56:40', 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1972-02-22 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1348436938662744064, 'yingzheng', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '嬴政', 1, '18168586750', NULL, NULL, NULL, 't', 'Y', '2021-01-18 16:06:24', '221.230.31.129', -1, '2021-01-11 09:09:54', -1, '2021-01-24 22:52:51', 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1999-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (1354366673393684480, 'sunwukong', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '孙悟空', 1, '18168586750', NULL, NULL, NULL, 't', 'S', NULL, NULL, -1, '2021-01-27 17:52:33', NULL, NULL, 'f', NULL, NULL, NULL, 0, 3, '[7864371]', '普通用户', 2, '1997-12-02 00:00:00');
INSERT INTO "public"."sysuser" VALUES (-1, 'system', '77AD8497B4F3D83D9A8373F2D80A02BA27E9A6B35A46D47E55CEBDC344D921AC', '超级管理员', 1, '18168586750', '843468011@qq.com', '843468011', 'rongguohao', 't', 'C', '2021-04-04 21:20:36', '117.90.246.241', -1, '2018-11-28 00:00:00', -1, '2021-04-04 21:24:06', 'f', 'https://haohaoplay-1253596932.cos.ap-nanjing.myqcloud.com/avatar/-1_1617086583', '', '镇江京口区', 0, 1, '[67108863]', '超级管理员', 0, '1992-12-02 00:00:00');
COMMIT;

-- ----------------------------
-- Primary Key structure for table sysdict
-- ----------------------------
ALTER TABLE "public"."sysdict" ADD CONSTRAINT "SysDict_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Indexes structure for table sysloginrecord
-- ----------------------------
CREATE INDEX "ix_userid_jwtexpiretime" ON "public"."sysloginrecord" USING btree (
  "userid" "pg_catalog"."int8_ops" ASC NULLS LAST,
  "jwtexpiretime" "pg_catalog"."timestamp_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table sysloginrecord
-- ----------------------------
ALTER TABLE "public"."sysloginrecord" ADD CONSTRAINT "SysLoginRecord_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Uniques structure for table sysmodule
-- ----------------------------
ALTER TABLE "public"."sysmodule" ADD CONSTRAINT "key_layer_number" UNIQUE ("layer", "number");

-- ----------------------------
-- Primary Key structure for table sysmodule
-- ----------------------------
ALTER TABLE "public"."sysmodule" ADD CONSTRAINT "sysapp_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Uniques structure for table sysrole
-- ----------------------------
ALTER TABLE "public"."sysrole" ADD CONSTRAINT "key_name" UNIQUE ("name");

-- ----------------------------
-- Primary Key structure for table sysrole
-- ----------------------------
ALTER TABLE "public"."sysrole" ADD CONSTRAINT "sysrole_pkey" PRIMARY KEY ("id");

-- ----------------------------
-- Indexes structure for table sysuser
-- ----------------------------
CREATE INDEX "key_roleid" ON "public"."sysuser" USING btree (
  "roleid" "pg_catalog"."int8_ops" ASC NULLS LAST
);

-- ----------------------------
-- Uniques structure for table sysuser
-- ----------------------------
ALTER TABLE "public"."sysuser" ADD CONSTRAINT "key_loginname" UNIQUE ("account");

-- ----------------------------
-- Primary Key structure for table sysuser
-- ----------------------------
ALTER TABLE "public"."sysuser" ADD CONSTRAINT "SysUser_pkey" PRIMARY KEY ("id");
