import requests
from bs4 import BeautifulSoup

with open('./html_files/Easy_CodeChef.html') as file:
    html = file.read()

soup = BeautifulSoup(html,'lxml')

datatable_tags = soup.select('table.dataTable')
datatable = datatable_tags[0]
prob_tags = datatable.select('a>b')
prob_names = [tag.getText() for tag in prob_tags]

for i in prob_names:
    print(i)

