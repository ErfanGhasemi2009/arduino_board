#include <DHT.h>
#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>

// تنظیمات نمایشگر OLED
#define SCREEN_WIDTH 128 // عرض نمایشگر OLED بر حسب پیکسل
#define SCREEN_HEIGHT 64 // ارتفاع نمایشگر OLED بر حسب پیکسل
#define OLED_RESET    -1 // در صورت عدم استفاده از پایه ریست، مقدار -1 را تنظیم کنید
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);

// تنظیمات سنسور DHT
#define DHTPIN 2     // پین متصل به داده سنسور
#define DHTTYPE DHT22   // تعریف نوع سنسور، در اینجا DHT22
DHT dht(DHTPIN, DHTTYPE);

// تنظیمات بازر
#define BUZZER_PIN 8  // پین متصل به بازر

void setup() {
  Serial.begin(9600);
  Serial.println("DHT22 and OLED display test!");

  dht.begin();
  
  // شروع نمایشگر OLED
  if(!display.begin(SSD1306_SWITCHCAPVCC, 0x3C)) { // آدرس I2C نمایشگر OLED
    Serial.println(F("SSD1306 allocation failed"));
    for(;;); // توقف در صورت شکست
  }

  // تنظیم پین بازر به عنوان خروجی
  pinMode(BUZZER_PIN, OUTPUT);

  // پاک کردن بافر نمایشگر
  display.clearDisplay();
}

void loop() {
  delay(2000);  // تاخیر 2 ثانیه‌ای برای خواندن پایدار داده‌ها

  // خواندن دما و رطوبت از سنسور
  float humidity = dht.readHumidity();
  float temperature = dht.readTemperature();

  // بررسی خطا در خواندن داده‌ها
  if (isnan(humidity) || isnan(temperature)) {
    Serial.println("Failed to read from DHT sensor!");
    display.clearDisplay();
    display.setTextSize(1);
    display.setTextColor(SSD1306_WHITE);
    display.setCursor(0, 0);
    display.println("Failed to read");
    display.println("from DHT sensor!");
    display.display();
    return;
  }

  // چاپ مقادیر دما و رطوبت در Serial Monitor
  Serial.print("Humidity: ");
  Serial.print(humidity);
  Serial.print(" %\t");
  Serial.print("Temperature: ");
  Serial.print(temperature);
  Serial.println(" *C");

  // نمایش مقادیر دما و رطوبت در نمایشگر OLED
  display.clearDisplay();
  display.setTextSize(1);
  display.setTextColor(SSD1306_WHITE);
  display.setCursor(0, 0);
  display.print("Humidity: ");
  display.print(humidity);
  display.println(" %");
  display.print("Temperature: ");
  display.print(temperature);
  display.println(" *C");
  display.display();

  // بررسی دمای بالای 30 درجه و فعال کردن بازر
  if (temperature > 30.0) {
    digitalWrite(BUZZER_PIN, HIGH); // فعال کردن بازر
  } else {
    digitalWrite(BUZZER_PIN, LOW); // غیرفعال کردن بازر
  }
}