SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL';

CREATE SCHEMA IF NOT EXISTS `hurksbestelsysteem` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci ;
USE `hurksbestelsysteem` ;

-- -----------------------------------------------------
-- Table `hurksbestelsysteem`.`product`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `hurksbestelsysteem`.`product` ;

CREATE  TABLE IF NOT EXISTS `hurksbestelsysteem`.`product` (
  `idproduct` INT NOT NULL AUTO_INCREMENT ,
  `productname` VARCHAR(45) NOT NULL ,
  `productcode` INT NOT NULL ,
  `description` TEXT NULL ,
  `price` DECIMAL(19,2) NULL ,
  `pricetype` ENUM('UNIT', 'WEIGHT') NULL ,
  PRIMARY KEY (`idproduct`) ,
  UNIQUE INDEX `productcode_UNIQUE` (`productcode` ASC) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `hurksbestelsysteem`.`category`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `hurksbestelsysteem`.`category` ;

CREATE  TABLE IF NOT EXISTS `hurksbestelsysteem`.`category` (
  `idcategory` INT NOT NULL AUTO_INCREMENT ,
  `categoryname` VARCHAR(45) NOT NULL ,
  `categorydescription` TEXT NULL ,
  PRIMARY KEY (`idcategory`) ,
  UNIQUE INDEX `categoryname_UNIQUE` (`categoryname` ASC) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `hurksbestelsysteem`.`product_category`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `hurksbestelsysteem`.`product_category` ;

CREATE  TABLE IF NOT EXISTS `hurksbestelsysteem`.`product_category` (
  `idproduct.category` INT NOT NULL AUTO_INCREMENT ,
  `categoryid` INT NOT NULL ,
  `productid` INT NOT NULL ,
  PRIMARY KEY (`idproduct.category`) ,
  INDEX `category` (`categoryid` ASC) ,
  INDEX `product` (`productid` ASC) ,
  CONSTRAINT `category`
    FOREIGN KEY (`categoryid` )
    REFERENCES `hurksbestelsysteem`.`category` (`idcategory` )
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT `product`
    FOREIGN KEY (`productid` )
    REFERENCES `hurksbestelsysteem`.`product` (`idproduct` )
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB;



SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
