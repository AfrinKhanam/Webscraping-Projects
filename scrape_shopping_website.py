from bs4 import BeautifulSoup
from selenium import webdriver
import pandas as pd

driver = webdriver.Firefox(executable_path=r'/home/ashutosh/Desktop/PythonWorkspace/PythonPractice/Webscrape/geckodriver-v0.27.0-linux64/geckodriver')
driver.get('https://www.flipkart.com/search?q=laptops&otracker=search&otracker1=search&marketplace=FLIPKART&as-show=on&as=off')
content = driver.page_source
soup = BeautifulSoup(content,'html5lib')

products = []
prices = []

for item in soup.find_all('a',href=True, attrs={'class':'_31qSD5'}):
    product = item.find('div',attrs={'class':'_3wU53n'})
    price = item.find('div', attrs={'class':'_1vC4OE'})
    products.append(product.text)
    prices.append(price.text)

df = pd.DataFrame({'product_name':products, 'price':prices})
df.to_csv('products.csv',index=False,encoding='utf-8')
