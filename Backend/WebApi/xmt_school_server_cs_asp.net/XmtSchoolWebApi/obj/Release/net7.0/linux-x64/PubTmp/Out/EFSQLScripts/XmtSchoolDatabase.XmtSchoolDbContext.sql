CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE TABLE `Users` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Type` tinyint unsigned NOT NULL,
        `Username` longtext NOT NULL,
        `Password` longtext NOT NULL,
        `Email` longtext NOT NULL,
        `AvatarUrl` longtext NOT NULL,
        PRIMARY KEY (`Id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE TABLE `Tests` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Name` longtext NOT NULL,
        `CreationDate` datetime(6) NOT NULL,
        `AuthorUserId` int NOT NULL,
        `StartDate` datetime(6) NOT NULL,
        `EndDate` datetime(6) NOT NULL,
        `RandomiseQuestionsOrder` tinyint(1) NOT NULL,
        `RandomiseAnswersOrder` tinyint(1) NOT NULL,
        PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Tests_Users_AuthorUserId` FOREIGN KEY (`AuthorUserId`) REFERENCES `Users` (`Id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE TABLE `Tokens` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `UserId` int NOT NULL,
        `TokenString` longtext NOT NULL,
        `LastUsed` datetime(6) NOT NULL,
        PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Tokens_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE TABLE `Marks` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `UserId` int NOT NULL,
        `TestId` int NOT NULL,
        `Points` decimal(18,2) NOT NULL,
        PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Marks_Tests_TestId` FOREIGN KEY (`TestId`) REFERENCES `Tests` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_Marks_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE TABLE `Questions` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `TestId` int NOT NULL,
        `QuestionText` longtext NOT NULL,
        PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Questions_Tests_TestId` FOREIGN KEY (`TestId`) REFERENCES `Tests` (`Id`) ON DELETE CASCADE
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE TABLE `Answers` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `QuestionId` int NOT NULL,
        `AnswerText` longtext NOT NULL,
        `IsValidAnswer` tinyint(1) NOT NULL,
        PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Answers_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `Questions` (`Id`) ON DELETE CASCADE
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE TABLE `SelectedAnswers` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `QuestionId` int NOT NULL,
        `AnswerId` int NOT NULL,
        `MarkId` int NULL,
        PRIMARY KEY (`Id`),
        CONSTRAINT `FK_SelectedAnswers_Answers_AnswerId` FOREIGN KEY (`AnswerId`) REFERENCES `Answers` (`Id`),
        CONSTRAINT `FK_SelectedAnswers_Marks_MarkId` FOREIGN KEY (`MarkId`) REFERENCES `Marks` (`Id`),
        CONSTRAINT `FK_SelectedAnswers_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `Questions` (`Id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE INDEX `IX_Answers_QuestionId` ON `Answers` (`QuestionId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE INDEX `IX_Marks_TestId` ON `Marks` (`TestId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE INDEX `IX_Marks_UserId` ON `Marks` (`UserId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE INDEX `IX_Questions_TestId` ON `Questions` (`TestId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE INDEX `IX_SelectedAnswers_AnswerId` ON `SelectedAnswers` (`AnswerId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE INDEX `IX_SelectedAnswers_MarkId` ON `SelectedAnswers` (`MarkId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE INDEX `IX_SelectedAnswers_QuestionId` ON `SelectedAnswers` (`QuestionId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE INDEX `IX_Tests_AuthorUserId` ON `Tests` (`AuthorUserId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    CREATE INDEX `IX_Tokens_UserId` ON `Tokens` (`UserId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230913154646_DbContextV1')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230913154646_DbContextV1', '7.0.13');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231103102431_DbContextV2')
BEGIN
    ALTER TABLE `Tests` MODIFY `Name` varchar(128) NOT NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231103102431_DbContextV2')
BEGIN
    ALTER TABLE `Questions` MODIFY `QuestionText` varchar(1024) NOT NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231103102431_DbContextV2')
BEGIN
    ALTER TABLE `Answers` MODIFY `AnswerText` varchar(512) NOT NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231103102431_DbContextV2')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231103102431_DbContextV2', '7.0.13');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217161118_DbContextV3')
BEGIN
    ALTER TABLE `Users` MODIFY `Username` varchar(32) NOT NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217161118_DbContextV3')
BEGIN
    ALTER TABLE `Users` MODIFY `Password` varchar(64) NOT NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217161118_DbContextV3')
BEGIN
    ALTER TABLE `Users` MODIFY `Email` varchar(128) NOT NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217161118_DbContextV3')
BEGIN
    ALTER TABLE `Users` MODIFY `AvatarUrl` varchar(512) NOT NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217161118_DbContextV3')
BEGIN
    CREATE UNIQUE INDEX `IX_Users_Username` ON `Users` (`Username`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217161118_DbContextV3')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231217161118_DbContextV3', '7.0.13');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217162803_DbContextV4')
BEGIN
    DROP INDEX IX_Users_Username ON Users;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217162803_DbContextV4')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231217162803_DbContextV4', '7.0.13');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217172209_DbContextV5')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231217172209_DbContextV5', '7.0.13');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217172353_DbContextV6')
BEGIN
    ALTER TABLE `Marks` DROP CONSTRAINT `FK_Marks_Users_UserId`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217172353_DbContextV6')
BEGIN
    ALTER TABLE `Marks` ADD CONSTRAINT `FK_Marks_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217172353_DbContextV6')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231217172353_DbContextV6', '7.0.13');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217172436_DbContextV7')
BEGIN
    ALTER TABLE `Marks` DROP CONSTRAINT `FK_Marks_Users_UserId`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217172436_DbContextV7')
BEGIN
    ALTER TABLE `Marks` ADD CONSTRAINT `FK_Marks_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217172436_DbContextV7')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231217172436_DbContextV7', '7.0.13');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217173247_DbContextV8')
BEGIN
    ALTER TABLE `Marks` DROP CONSTRAINT `FK_Marks_Users_UserId`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217173247_DbContextV8')
BEGIN
    ALTER TABLE `Marks` ADD CONSTRAINT `FK_Marks_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217173247_DbContextV8')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231217173247_DbContextV8', '7.0.13');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217173848_DbContextV9')
BEGIN
    ALTER TABLE `SelectedAnswers` DROP CONSTRAINT `FK_SelectedAnswers_Marks_MarkId`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217173848_DbContextV9')
BEGIN
    ALTER TABLE `SelectedAnswers` MODIFY `MarkId` int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217173848_DbContextV9')
BEGIN
    ALTER TABLE `SelectedAnswers` ADD CONSTRAINT `FK_SelectedAnswers_Marks_MarkId` FOREIGN KEY (`MarkId`) REFERENCES `Marks` (`Id`) ON DELETE CASCADE;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231217173848_DbContextV9')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231217173848_DbContextV9', '7.0.13');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124173716_DbContextV10')
BEGIN
    ALTER TABLE `Answers` DROP CONSTRAINT `FK_Answers_Questions_QuestionId`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124173716_DbContextV10')
BEGIN
    ALTER TABLE `Questions` DROP CONSTRAINT `FK_Questions_Tests_TestId`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124173716_DbContextV10')
BEGIN
    ALTER TABLE `Answers` ADD CONSTRAINT `FK_Answers_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `Questions` (`Id`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124173716_DbContextV10')
BEGIN
    ALTER TABLE `Questions` ADD CONSTRAINT `FK_Questions_Tests_TestId` FOREIGN KEY (`TestId`) REFERENCES `Tests` (`Id`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124173716_DbContextV10')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20240124173716_DbContextV10', '7.0.13');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124173839_DbContextV11')
BEGIN
    ALTER TABLE `Answers` DROP CONSTRAINT `FK_Answers_Questions_QuestionId`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124173839_DbContextV11')
BEGIN
    ALTER TABLE `Questions` DROP CONSTRAINT `FK_Questions_Tests_TestId`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124173839_DbContextV11')
BEGIN
    ALTER TABLE `Answers` ADD CONSTRAINT `FK_Answers_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `Questions` (`Id`) ON DELETE CASCADE;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124173839_DbContextV11')
BEGIN
    ALTER TABLE `Questions` ADD CONSTRAINT `FK_Questions_Tests_TestId` FOREIGN KEY (`TestId`) REFERENCES `Tests` (`Id`) ON DELETE CASCADE;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124173839_DbContextV11')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20240124173839_DbContextV11', '7.0.13');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124182025_DbContextV12')
BEGIN
    ALTER TABLE `Answers` DROP CONSTRAINT `FK_Answers_Questions_QuestionId`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124182025_DbContextV12')
BEGIN
    ALTER TABLE `Answers` ADD CONSTRAINT `FK_Answers_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `Questions` (`Id`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124182025_DbContextV12')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20240124182025_DbContextV12', '7.0.13');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124183021_DbContextV13')
BEGIN
    ALTER TABLE `Answers` DROP CONSTRAINT `FK_Answers_Questions_QuestionId`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124183021_DbContextV13')
BEGIN
    ALTER TABLE `Answers` ADD CONSTRAINT `FK_Answers_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `Questions` (`Id`) ON DELETE CASCADE;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124183021_DbContextV13')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20240124183021_DbContextV13', '7.0.13');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20240124191136_DbContextV14')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20240124191136_DbContextV14', '7.0.13');
END;

COMMIT;

