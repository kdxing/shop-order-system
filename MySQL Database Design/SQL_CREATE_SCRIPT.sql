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
  `idproduct_category` INT NOT NULL AUTO_INCREMENT ,
  `categoryid` INT NOT NULL ,
  `productid` INT NOT NULL ,
  PRIMARY KEY (`idproduct_category`) ,
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


-- -----------------------------------------------------
-- Table `hurksbestelsysteem`.`employee`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `hurksbestelsysteem`.`employee` ;

CREATE  TABLE IF NOT EXISTS `hurksbestelsysteem`.`employee` (
  `idemployee` INT NOT NULL AUTO_INCREMENT ,
  `name` VARCHAR(45) NOT NULL ,
  PRIMARY KEY (`idemployee`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `hurksbestelsysteem`.`customer`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `hurksbestelsysteem`.`customer` ;

CREATE  TABLE IF NOT EXISTS `hurksbestelsysteem`.`customer` (
  `idcustomer` INT NOT NULL AUTO_INCREMENT ,
  `firstname` VARCHAR(45) NULL ,
  `lastname` VARCHAR(45) NOT NULL ,
  `phonenumber` VARCHAR(45) NULL ,
  `street` VARCHAR(45) NULL ,
  `streetnumber` VARCHAR(45) NULL ,
  `town` VARCHAR(45) NULL ,
  PRIMARY KEY (`idcustomer`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `hurksbestelsysteem`.`order`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `hurksbestelsysteem`.`order` ;

CREATE  TABLE IF NOT EXISTS `hurksbestelsysteem`.`order` (
  `idorder` INT NOT NULL AUTO_INCREMENT ,
  `customerid` INT NOT NULL ,
  `ordered_datetime` DATETIME NOT NULL ,
  `pickup_datetime` DATETIME NOT NULL ,
  `employee_id` INT NOT NULL ,
  `description` VARCHAR(45) NULL ,
  PRIMARY KEY (`idorder`) ,
  INDEX `employee_id` (`employee_id` ASC) ,
  INDEX `customerid` (`customerid` ASC) ,
  CONSTRAINT `employee_id`
    FOREIGN KEY (`employee_id` )
    REFERENCES `hurksbestelsysteem`.`employee` (`idemployee` )
    ON DELETE RESTRICT
    ON UPDATE NO ACTION,
  CONSTRAINT `customerid`
    FOREIGN KEY (`customerid` )
    REFERENCES `hurksbestelsysteem`.`customer` (`idcustomer` )
    ON DELETE RESTRICT
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `hurksbestelsysteem`.`order_entry`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `hurksbestelsysteem`.`order_entry` ;

CREATE  TABLE IF NOT EXISTS `hurksbestelsysteem`.`order_entry` (
  `idorder_entry` INT NOT NULL AUTO_INCREMENT ,
  `orderid` INT NOT NULL ,
  `productid` INT NOT NULL ,
  `quantity` INT NOT NULL ,
  PRIMARY KEY (`idorder_entry`) ,
  INDEX `orderid` (`orderid` ASC) ,
  INDEX `productid` (`productid` ASC) ,
  CONSTRAINT `orderid`
    FOREIGN KEY (`orderid` )
    REFERENCES `hurksbestelsysteem`.`order` (`idorder` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `productid`
    FOREIGN KEY (`productid` )
    REFERENCES `hurksbestelsysteem`.`product` (`idproduct` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;



SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
