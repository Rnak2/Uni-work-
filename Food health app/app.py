from flask import Flask, render_template, request
from tensorflow.keras.models import load_model
from tensorflow.keras.preprocessing import image
import numpy as np
import json
import os

app = Flask(__name__)

# Load model
model = load_model('model.h5')


# Load class names
with open('class_names.txt', 'r') as f:
    class_names = [line.strip() for line in f.readlines()]

# Load healthy/unhealthy mapping
with open('healthy_unhealthy.json', 'r') as f:
    health_map = json.load(f)

# Upload folder
UPLOAD_FOLDER = 'static/uploads'
os.makedirs(UPLOAD_FOLDER, exist_ok=True)
app.config['UPLOAD_FOLDER'] = UPLOAD_FOLDER

# Preprocess image
def prepare_image(img_path):
    img = image.load_img(img_path, target_size=(64, 64))
    img_array = image.img_to_array(img)
    img_array = np.expand_dims(img_array, axis=0) / 255.0
    return img_array

@app.route("/", methods=["GET", "POST"])
def index():
    if request.method == "POST":
        file = request.files['image']
        if file:
            file_path = os.path.join(app.config['UPLOAD_FOLDER'], file.filename)
            file.save(file_path)
            
            # Predict food class
            img_array = prepare_image(file_path)
            predictions = model.predict(img_array)
            predicted_class = class_names[np.argmax(predictions[0])]
            confidence = round(100 * np.max(predictions[0]), 2)
            
            # Map to health label
            health_status = health_map.get(predicted_class, "Unknown")

            return render_template("index.html", file_path=file_path, predicted_class=predicted_class, confidence=confidence, health_status=health_status)
    
    return render_template("index.html")

if __name__ == "__main__":
    app.run(debug=True)
