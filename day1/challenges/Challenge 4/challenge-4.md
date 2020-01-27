# Challenge 4: VM - Managed Disks: Attach a Data Disk to a VM - Extend and Retype it.
[back](../../readme.md)  

## Here is what you will learn ##

## Create a data disk and attach it to a vm ##
```
[Azure Portal] -> Virtual machines -> e.g. 'vmweb01' -> Disks -> "+ Add data disk"
```  
| Name | Value |
|---|---|
| LUN (aka _logical unit number_)  |  e.g. 0 |
| Name  | **_create disk_** |
| Disk name  |  **vmweb01-datadisk1** |   
| Resource group  |  rg-contosomortgage-www |   
| Source type  |  None |   
| Size  | **_Change size_** |
| Account type | **Standard SSD** |
| Size  |  **128GiB (E10)** | 
| Host caching  |  **Read/write** | 

**Don't forget to press the save button!**

## Logon to your windows vm and partition the disk ##
Once logged into the vm - execute **_diskmgmt.msc_** to open the **Disk Manager**. Your attached data disk will show up 'uninitialized':  
![alt](https://link)


> Questions:  
> - How much is a E10 / month? (fix price, variable costs, region differences)
> - Can a disk be resized without losing its data? 