from selenium import webdriver
browser = webdriver.Firefox(executable_path=r'/home/ashutosh/Desktop/PythonWorkspace/PythonPractice/Webscrape/geckodriver-v0.27.0-linux64/geckodriver')
browser.get('https://stackoverflow.com/questions?sort=votes')

title = browser.find_element_by_css_selector('h1').text

questions = browser.find_elements_by_css_selector('.question-summary')

for question in questions:
    quest = question.find_element_by_css_selector('.summary h3 a').text
    answer = question.find_element_by_css_selector('.summary .excerpt').text
    views = question.find_element_by_css_selector('.question-summary .statscontainer .views').text
    votes = question.find_element_by_css_selector('.question-summary .statscontainer .stats .vote .votes .vote-count-post').text
    print("-------------------------------------------------------------------------")
    print("question is: ",quest)
    print("answer is: ",answer)
    print("views is: ",views)
    print("votes is: ",votes)
    print("-------------------------------------------------------------------------")
