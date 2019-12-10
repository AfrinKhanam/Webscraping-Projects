from rasa_nlu.model import Interpreter
import json
interpreter = Interpreter.load("./models/current/model_20181227-183200")
message = "hey"
result = interpreter.parse(message)
print(json.dumps(result, indent=2))