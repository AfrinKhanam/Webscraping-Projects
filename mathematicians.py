import requests
from bs4 import BeautifulSoup

with open('famous_mathematicians.html') as file:
    html = file.read()

soup = BeautifulSoup(html,'html.parser')

names = set()
for idx,li in enumerate(soup.select('li')):
    # print(idx,name.text)
    for name in li.text.split('\n'):
        if name:
            names.add(name.strip())
print(names)