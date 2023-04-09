import time
from time import sleep
from datetime import datetime
from selenium import webdriver
from selenium.common.exceptions import NoSuchElementException, StaleElementReferenceException
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait as wait
from selenium.webdriver.support import expected_conditions as EC
from bs4 import BeautifulSoup
import os
import requests
import pandas as pd
import json
import subprocess

chrome_options= webdriver.ChromeOptions()
chrome_options.add_argument('--headless')
chrome_options.add_argument('--no-sandbox')
chrome_options.add_argument("--remote_debugging-port=9222")
chrome_options.add_argument('--disable-dev-shm-usage')
df = pd.read_excel("gisaid_cov2020_acknowledgement_table.xls", header=3)
id_list=df.iloc[:,0]
os.chdir('******')
log_file = open('******','w')
file_list = os.listdir('******')    #list about already downloaded
whole = len(id_list)-len(file_list)
current = 0
alarm = 0
fail=0

def Time_now():
    now = datetime.now()
    Date_time = ("%s-%s-%s %s:%s:%s " % (now.year, now.month, now.day, now.hour, now.minute, now.second))
    return Date_time
    
def Mes(text):
    text = Time_now()+text
    log_file.write(text+'\n')
    print(text+'\n')

def Search(driver,accession):
    try:    #searching
        elem=wait(driver,20).until(
            EC.presence_of_element_located((By.XPATH,"/html/body/form/div[5]/div/div[2]/div/div[2]/div[1]/div[2]/table/tbody/tr/td[1]/table[1]/tbody/tr/td[2]/div/div[1]/input"))
            )
        elem.clear()
        elem.send_keys(accession)
        
    except:
        driver.quit()
        Mes('search error')
        return 'Restart'
        
    Mes('search '+accession)
    sleep(7)
    
    try:    #loading
        elem = wait(driver, 60).until(
            EC.text_to_be_present_in_element((By.XPATH,"/html/body/form/div[5]/div/div[2]/div/div[2]/div[2]/div/div[3]/table/tbody[2]/tr/td[3]/div"),accession)
            )
        elem = driver.find_element_by_xpath("/html/body/form/div[5]/div/div[2]/div/div[2]/div[2]/div/div[3]/table/tbody[2]/tr/td[2]/div")
        elem.click()
                
    except:
        driver.quit()
        Mes('loading error')
        return 'Restart'
    
    Mes(accession+' loading')
    sleep(3)
    
    try:    #frame changing
        iframe = wait(driver, 30).until(
            EC.presence_of_element_located((By.TAG_NAME,'iframe'))
            )
        driver.switch_to.frame(iframe)
        
    except:
        Mes('Frame error')
        return 'Error'
    
    sleep(5)
    
    try:    #fasta
        elem=driver.find_elements_by_tag_name('button')
        elem[2].click()
        
    except:
        Mes('Download error')
        driver.back()
        return 'Error'
    
    sleep(3)
    #meta
    tbody=driver.find_element_by_xpath('/html/body/form/div[5]/div/div[1]/div/div/table/tbody')
    data=tbody.find_elements_by_tag_name('tr')
    meta={}
    for i in data:
        try:
            td=i.find_elements_by_tag_name('td')
            txt=td[0].find_element_by_tag_name('b')
            meta[txt.text]=td[1].text
        except:
            pass
    f=open('******'+accession+'.txt','w')
    f.write(json.dumps(meta, indent=4))
    f.close()
    
    driver.back() #back
    return 'Success'

def Main():
    global fail
    if fail>10:
        Mes('Zzz..')
        sleep(300)
        fail=0
    #driver = webdriver.Chrome(executable_path="******") #for laptop
    driver = webdriver.Chrome(chrome_options=chrome_options)   #for server
    driver.get("https://www.gisaid.org/")
    Mes('Open')
    sleep(3)

    try:    #click login
        elem = wait(driver, 5).until(
            EC.presence_of_element_located((By.CLASS_NAME,'login'))
            )
        elem.click()
    
    except:
        Mes('Failed to try login')
        driver.quit()
        a+=1    #make error to restart
    
    sleep(3)
    
    try:    #login
        elem = wait(driver, 20).until(
            EC.presence_of_element_located((By.ID,'elogin'))
            )
        sleep(1)
        elem.clear()
        elem.send_keys("******")    #Type ID
        sleep(1)
        elem = driver.find_element_by_name("password")
        elem.clear()
        elem.send_keys("******")    #Type password
        
        sleep(5)
        elem = wait(driver, 5).until(
            EC.element_to_be_clickable((By.CLASS_NAME,"form_button_submit"))
            )
        elem.click()
        Mes('Log in')
    
    except:
        Mes('Failed to login')
        driver.quit()
        fail+=1
        a+=1    #make error to restart
        
    sleep(5)
    
    try:    #tab change
        elem = wait(driver,40).until(
            EC.presence_of_element_located((By.XPATH,"/html/body/form/div[5]/div/div[1]/div/div/div/div/div[2]/div/ul/li[3]/a"))
            )
        elem.click()
        Mes('Tab change')
        
    except:
        Mes('Failed to change tab')
        driver.quit()
        fail+=1
        a+=1    #make error to restart
    
    sleep(7)
    
    try:    #browse
        list_buttons = driver.find_elements_by_class_name("sys-actionbar-action")
        list_buttons[1].click()
        Mes('Browse')
    
    except:
        Mes('browse error')
        driver.quit()
        fail+=1
        a+=1    #make error to restart
    fail=0
    Mes('Start to search')
    
    error=[]    #list for error case
    
    global whole, current, alarm

    f=open('******','a')
    
    CMD = "wall Start to search on GISAID parsing"
    out = subprocess.check_output(CMD,shell=True)
    
    for i in id_list:    #full ID list
        if not i+'.fasta' in file_list:    #except ID that already downloaded
            result = Search(driver,i)
            if result == 'Restart':    #error that can't be solved
                a+=1    #make error to restart
            
            elif result == 'Success':    #Success
                f.write(i+'\n')
                Mes(i+' success')
                current+=1
                percent = current/whole*100
                if int(percent)>alarm:
                    Mes(str(percent)+'% complete')
                    CMD = "wall GISAID parsing "+str(percent)+"% complete"
                    out = subprocess.check_output(CMD,shell=True)
                    alarm = int(percent)
                
            elif result == 'Error':    #error that can be solved
                error.append(i)
                Mes(i+' error')
                
    Mes("Start to solve errors")
    CMD = "wall Start to solve errors on GISAID parsing"
    out = subprocess.check_output(CMD,shell=True)
    
    while len(error)>0:    #until no error case
        for i in error:
            result = Search(driver,i)
            if result == 'Restart':    #error that can't be solved
                a+=1    #make error to restart
            
            elif result == 'Success':    #Success
                f.write(i+'\n')
                error.remove(i)
                Mes(i+' success')
                current+=1
                percent = current/whole*100
                if int(percent/10)>alarm:
                    Mes('Error '+str(percent)+'% complete')
                    #CMD = "wall Solving errors on GISAID parsing "+str(percent)+"% complete"
                    #out = subprocess.check_output(CMD,shell=True)
                    alarm = int(percent/10)
            
            elif result == 'Error':    #error that can be solved
                Mes(i+' re-error')
                return
                
    f.close()
    driver.quit()
    Mes('end')
    log_file.close()

while True:
    try:
        Main()
        break
    except:
        Mes('Restart')

CMD = "wall Finished GISAID parsing"
out = subprocess.check_output(CMD,shell=True)
