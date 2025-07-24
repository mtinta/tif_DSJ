import cv2
import json
import os

def detectar_formas(imagen_path, output_json, output_png):
    imagen_original = cv2.imread(imagen_path)
    if imagen_original is None:
        print(f"No se pudo cargar la imagen: {imagen_path}")
        return

    # Redimensionar la imagen a 1920x1080
    imagen_redimensionada = cv2.resize(imagen_original, (1920, 1080))

    # Guardar la imagen como PNG sin líneas de procesamiento
    cv2.imwrite(output_png, imagen_redimensionada)

    # Continuar con el procesamiento para detectar contornos (sin afectar la imagen guardada)
    gris = cv2.cvtColor(imagen_redimensionada, cv2.COLOR_BGR2GRAY)
    desenfoque = cv2.GaussianBlur(gris, (5, 5), 0)
    bordes = cv2.Canny(desenfoque, 50, 150)
    contornos, _ = cv2.findContours(bordes, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

    formas = {}
    for i, contorno in enumerate(contornos):
        epsilon = 0.02 * cv2.arcLength(contorno, True)
        aproximacion = cv2.approxPolyDP(contorno, epsilon, True)
        puntos = [{"x": int(p[0][0]), "y": int(p[0][1])} for p in aproximacion]
        formas[f'forma_{i+1}'] = puntos

    # Guardar los puntos detectados en JSON
    with open(output_json, 'w') as archivo_json:
        json.dump(formas, archivo_json, indent=4)

    print(f"[✔] Procesado: {os.path.basename(imagen_path)} → {os.path.basename(output_json)}, {os.path.basename(output_png)}")

# Carpeta de entrada y salida
carpeta_imagenes = "./imagenes"
carpeta_json = "./salida_json"
carpeta_png = "./salida_png"

# Crear carpetas de salida si no existen
os.makedirs(carpeta_json, exist_ok=True)
os.makedirs(carpeta_png, exist_ok=True)

# Extensiones permitidas
extensiones = (".jpg", ".jpeg", ".png", ".bmp")

# Procesar todas las imágenes de la carpeta
for archivo in os.listdir(carpeta_imagenes):
    if archivo.lower().endswith(extensiones):
        ruta_imagen = os.path.join(carpeta_imagenes, archivo)
        nombre_base = os.path.splitext(archivo)[0]
        ruta_json = os.path.join(carpeta_json, f"{nombre_base}.json")
        ruta_png = os.path.join(carpeta_png, f"{nombre_base}.png")
        detectar_formas(ruta_imagen, ruta_json, ruta_png)

print("\n🎉 ¡Todas las imágenes han sido procesadas y convertidas a PNG (sin marcas)!\n")
