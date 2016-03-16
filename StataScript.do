/* 	This program is associated with "Merging data fetched by the eBay fetch bot"
*/
	
* Program Version 1.0
* Last update 25/June/2014

clear 														// Leave matrix uncleared
clear mata
macro drop _all
program drop _all
set varabbrev off

******************************************************
******************  Rolling-window  ******************
******************************************************
****************** Program Settings ******************
global itemInfoFolder 	= "D:\Temp\ItemInfo"		// Location of the item info folder
global itemSpecsFolder 	= "D:\Temp\ItemSpecifics"	// Location of the item info folder
global itemBidsFolder 	= "D:\Temp\ItemBids"		// Location of the item info folder
global Folder			= "D:\Temp" 				// Folder to save the merged dataset
global outputFolder 	= "D:\Dropbox\[Share]\[Share] Xiaogang\eBayFetch\Dataset\" 				// Folder where the output spreadsheet should be saved				
******************************************************

* PROGRAM STARTS HERE *
* Other environmental variables
cap set procs_use 4
cap set memory 12g
#delimit cr
set trace off
set more off
cap set maxvar 20000
cap set matsize 11000
set scrollbufsize 2048000
set dp period
set scheme s1color
set printcolor automatic
set copycolor automatic
set autotabgraphs on
set level 95
set maxiter 16000
set varabbrev off
set reventries 32000
*set maxdb 500
set seed 999						
set type double
set logtype text
pause on
*sysdir set PERSONAL "$Data\ado"
*sysdir set PLUS "$Data\ado\plus"
*sysdir set OLDPLACE "$Data\ado" 
*net set ado PERSONAL
set rmsg on
cd "$Folder"

cap program drop mergeDataFile
program mergeDataFile
args filePath
	// check if the main data file exists
	capture confirm file "`filePath'"
	if _rc==0 {
	*		// tostring all variables
	*		ds
	*		if "`r(varlist)'"~="" {
	*			foreach x in `r(varlist)' {
	*				tostring `x', replace
	*			}	
	*		}
		
		save "$Folder\\temp.dta", replace
		use "`filePath'", clear
	*		// tostring all variables
	*		ds
	*		if "`r(varlist)'"~="" {
	*			foreach x in `r(varlist)' {
	*				tostring `x', replace
	*			}	
	*		}	
		cap drop v2
		append using "$Folder\\temp.dta"
		duplicates drop
		order ItemID
		save "`filePath'", replace
	}
	else {
		display "The file `filePath' does not exist, creating a new dataset"
		duplicates drop
		order ItemID
		save "`filePath'", replace
	}
end program

disp "Stage 1: Listing Info Variables"
// Consolidate variables of item info
	local myfiles: dir "$itemInfoFolder\\\" files "*.txt"
	foreach x of local myfiles {
		import delimited "$itemInfoFolder\\`x'", delimiter("-+|+-", asstring) bindquote(strict) varnames(1) stripquote(yes) case(preserve) clear 
		duplicates drop
		cap erase "$itemInfoFolder\\`x'"
		save "$itemInfoFolder\\Item`x'.dta", replace
	}	
	
	local i = 1
	foreach x of local myfiles {
		if `i'==1 {
			disp "Now processing to file `x'"
			use "$itemInfoFolder\\Item`x'.dta", clear
			erase "$itemInfoFolder\\Item`x'.dta"
		}
		else {
			
			cap merge 1:1 ItemID using "$itemInfoFolder\\Item`x'.dta", nogen
			erase "$itemInfoFolder\\Item`x'.dta"
		}
		local i = `i' + 1
	}		
	
	cap tostring SellerVacationNote, replace
	
	mergeDataFile "$Folder\\ListingInfo.dta"
	export excel using "$outputFolder\\ListingInfo.xlsx", firstrow(variables) replace

// Consolidate variables of item Specifics
	local myfiles: dir "$itemSpecsFolder\\\" files "*.txt"
	foreach x of local myfiles {
		import delimited "$itemSpecsFolder\\`x'", delimiter("-+|+-", asstring) bindquote(strict) varnames(1) stripquote(yes) case(preserve) clear 
		duplicates drop
		cap erase "$itemSpecsFolder\\`x'"
		save "$itemSpecsFolder\\Item`x'.dta", replace
	}	

	local myfiles: dir "$itemSpecsFolder\\\" files "*.dta"
	local i = 1
	foreach x of local myfiles {
		if `i'==1 {
			disp "Now processing to file `x'"
			use "$itemSpecsFolder\\`x'", clear
			erase "$itemSpecsFolder\\`x'"
		}
		else {
			cap merge 1:1 ItemID using "$itemSpecsFolder\\`x'", nogen
			erase "$itemSpecsFolder\\`x'"
		}
		local i = `i' + 1
	}		
	
	mergeDataFile "$Folder\\ItemSpecifics.dta"	
	export excel using "$outputFolder\\ItemSpecifics.xlsx", firstrow(variables) replace

// Consolidate variables of Bidder History
	capture confirm file "$Folder\\BidderHistory.csv"
	if _rc==0 {
		import delimited "$Folder\\BidderHistory.csv", delimiter(",", asstring) bindquote(strict) varnames(1) stripquote(yes) case(preserve) clear 
		mergeDataFile "$Folder\\BidderHistory.dta"	
		erase "$Folder\\BidderHistory.csv"
		export excel using "$outputFolder\\BidderHistory.xlsx", firstrow(variables) replace
	}


// Consolidate variables of Item Bids
	local myfiles: dir "$itemBidsFolder\\\" files "*.txt"
	foreach x of local myfiles {
		import delimited "$itemBidsFolder\\`x'", delimiter("-+|+-", asstring) bindquote(strict) varnames(1) stripquote(yes) case(preserve) clear 
		duplicates drop
		cap erase "$itemBidsFolder\\`x'"
		save "$itemBidsFolder\\Item`x'.dta", replace
	}	

	local i = 1
	foreach x of local myfiles {
		if `i'==1 {
			disp "Now processing to file `x'"
			use "$itemBidsFolder\\Item`x'.dta", clear
			erase "$itemBidsFolder\\Item`x'.dta"
		}
		else {
			
			cap merge 1:1 ItemID using "$itemBidsFolder\\Item`x'.dta", nogen
			erase "$itemBidsFolder\\Item`x'.dta"
		}
		local i = `i' + 1
	}		
	
	mergeDataFile "$Folder\\ItemBids.dta"	
	export excel using "$outputFolder\\ItemBids.xlsx", firstrow(variables) replace
	
