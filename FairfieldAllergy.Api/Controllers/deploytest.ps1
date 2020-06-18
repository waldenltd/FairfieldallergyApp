npm run test

#Backup your cuurent test programs
#1. Create string consisting of backup folder and current date and time
$string1 = "\\FCAAP02\FairfieldAllergyBackups\backup_" + "$((Get-Date).ToString('yyyy_MM_dd_HH_mm'))"
Write-Output $string1
#2. Create Backup folder on remote box
New-Item -ItemType Directory -Path $string1
#Copy current test enviornment to backup folder
Copy-Item -Path '\\FCAAP02\FairfieldAllergyTest\*' -Recurse -Destination $string1

#Remove current version in test
Remove-Item -Path '\\FCAAP02\FairfieldAllergyTest\FairfieldAllergyAdmin\img' -Recurse
Remove-Item -Path '\\FCAAP02\FairfieldAllergyTest\FairfieldAllergyAdmin\js' -Recurse
Remove-Item -Path '\\FCAAP02\FairfieldAllergyTest\FairfieldAllergyAdmin\favicon.ico' -Force
Remove-Item -Path '\\FCAAP02\FairfieldAllergyTest\FairfieldAllergyAdmin\index.html' -Force
Remove-Item -Path '\\FCAAP02\FairfieldAllergyTest\FairfieldAllergyAdmin\manifest.json' -Force
Remove-Item -Path '\\FCAAP02\FairfieldAllergyTest\FairfieldAllergyAdmin\robots.txt' -Force

Copy-Item -Path 'C:\ionicwithvuedev\fairfield-app-admin\dist\*' -Recurse -Destination '\\FCAAP02\FairfieldAllergyTest\FairfieldAllergyAdmin'
#Remove-Item HKLM:\Software\MyCompany\OldApp -Recurse

#Copy current development version to test
Copy-Item -Path 'C:\OneDriveForBusiness Files\OneDrive - Walden Associates LTD\Desktop\ionicwithvuedev\fairfield-app-admin\dist\*' -Recurse -Destination '\\FCAAP02\FairfieldAllergyTest\FairfieldAllergyAdmin'


#Copy current development version to test
Copy-Item -Path 'C:\OneDriveForBusiness Files\OneDrive - Walden Associates LTD\Desktop\ionicwithvuedev\fairfield-app-admin\dist\*' -Recurse -Destination '\\FCAAP02\FairfieldAllergyTest\FairfieldAllergyAdmin'


#$string1 = "\\FCAAP02\FairfieldAllergyBackups" + ".\$((Get-Date).ToString('yyyy-MM-dd'))"

#New-Item -ItemType Directory -Path "\\FCAAP02\FairfieldAllergyBackups" + ".\$((Get-Date).ToString('yyyy-MM-dd'))"

#new-item \\FCAAP02\FairfieldAllergyBackups\test1 -itemtype directory

#Copy-Item -Filter *.txt -Path 'D:\temp\Test Folder' -Recurse -Destination 'D:\temp\Test Folder1'

#C:\ionicwithvuedev\fairfield-app-admin\dist

dotnet publish --output "c:\waldenltd\FairFieldRelease" --configuration release

Remove-Item '\\FCAAP02\FairfieldAllergyTest\FairfieldAllergyAdmin\fairfieldallergyapi\v1\*' -Recurse -Force

Copy-Item -Path 'c:\waldenltd\FairFieldRelease\*' -Recurse -Destination \\FCAAP02\FairfieldAllergyTest\FairfieldAllergyAdmin\fairfieldallergyapi\v1

