import requests
from bs4 import BeautifulSoup
import json


with open('/home/ashutosh/Desktop/indian_bank.html') as file:
    html = file.read()

soup = BeautifulSoup(html,features='html5lib')

main_content = soup.find('ul',attrs={"id":'responsive-menu'})
scrollbar_menus = []
# --------------------------fetch first menu---------------------------------------------
def metchod1(main_content):
    for idx,element in enumerate(main_content.contents):
        if element.name is not None:
            menu = element.find('a').get_text()
            if len(list(element.children)) is 1:
                scrollbar_menus.append({menu:element.find('a')['href']})
            else:
                element.a.decompose()
                scrollbar_menus.append({menu:element})
    return scrollbar_menus





# scrollbar_menus = method1(main_content)

# method1(scrollbar_menus[1]['About us▼'])
# -----------------------------fetch sub menu------------------------------------------
            
# for ele in scrollbar_menus:
#     for key,value in ele.items():
#         if isinstance(value,list):
#             # print(f'{key} ---> {value}')
#             for items in value: 
#                 if items.name is not None:
#                     for sub_ele in items.contents:
#                         if sub_ele.name is not None:
#                             submenu = sub_ele.find('a').get_text()
#                             if len(list(sub_ele.children)) is 1:
#                                 ele[key].append({submenu:sub_ele.find('a')['href']})
#                             else:
#                                 sub_ele.a.decompose()

#                                 ele[key].append({submenu:sub_ele.contents})
                # break
# ------------------------------fetch super sub menu-----------------------------------------

# for ele in scrollbar_menus:
#     for key,values in ele.items():
#         if isinstance(values,list):
#             for item in values:
#                 if isinstance(item,dict):
#                     for k,v in item.items():
#                         if isinstance(v,list):
#                             # print(f'{k}--->{v}')
#                             for sub_ele in v[0].contents:
#                                 if sub_ele.name is not None:
#                                     submenu = sub_ele.find('a').get_text()
#                                     if len(list(sub_ele.children)) is 1:
#                                         item[k].append({submenu:sub_ele.find('a')['href']})
#                                     else:
#                                         sub_ele.a.decompose()
#                                         item[k].append({submenu:sub_ele.contents})

# --------------------------remove junk data--------------------------------------------------
# for ele in scrollbar_menus:
#     for item,value in ele.items():
#         if isinstance(value,list):
#             del value[0]
#             for e in value:
#                 for k,v in e.items():
#                     if isinstance(v,list):
#                         del v[0]

# ---------------------------------------------------------------------------------
with open('scrollbar.json','w+') as file:
    file.write(str(scrollbar_menus))

print(scrollbar_menus)

        

