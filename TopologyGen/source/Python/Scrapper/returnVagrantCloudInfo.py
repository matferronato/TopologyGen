
import sys
import urllib.request
from bs4 import BeautifulSoup
import time

OSset = set()
if len(sys.argv) > 1:
    extraStuff = sys.argv[1]
    if "!" in extraStuff:
        extraStuff = extraStuff.replace("!","")
        extraStuff = "?utf8=&sort=downloads&provider=&q="+extraStuff
else:
    extraStuff = ""

url= "https://app.vagrantup.com/boxes/search"+extraStuff

print(extraStuff)
print(url)
logUrl = open("log.txt", "a")
logUrl.write(url+"\n")
logUrl.close()

page = urllib.request.urlopen(url)
webpageHtml = page.read()
soup = BeautifulSoup(webpageHtml, features="html.parser")
page.close()
container = soup.find("div")
divs = soup.findAll('div')
for eachDiv in divs:
    for j, eachWord in enumerate(str(eachDiv).split()):
        if "<h4>" in eachWord:
            OSset.add(str(str(eachDiv).split()[j+1]+"!"+str(eachDiv).split()[j+3]))

fileOs = open("../../../Automate/vagrant_box/boxes_to_download.txt", "w")
fileOsV = open("../../../Automate/vagrant_box/boxes_to_download_v.txt", "w")
with open("../../../Automate/vagrant_box/boxes_to_download.txt", "w") as filetowrite:
    for eachKey in OSset:
        osinfo = eachKey.split("!")
        os = osinfo[0]
        osV = osinfo[1]
        fileOs.write(os+"\n")
        fileOsV.write(osV+"\n")
fileOs.close()
fileOsV.close()
