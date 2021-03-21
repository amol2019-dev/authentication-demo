import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    var req = {
      "Username": "admin",
      "Password": "admin"
    };

    localStorage.setItem('jwttoken', '');

    http.post<User>(baseUrl + 'Users/authenticate', req).subscribe(result => {
      if (result.token !== null) {
        localStorage.setItem('jwttoken', result.token);
      }
      console.log(result);

      http.post<User>(baseUrl + 'Users/GetAll', req).subscribe(result => {
        console.log('got the result', result);
      }, error => console.error(error));

      http.get<WeatherForecast[]>(baseUrl + 'weatherforecast').subscribe(result => {
        this.forecasts = result;
      }, error => console.error(error));

      http.post<WeatherForecast>(baseUrl + 'weatherforecast/GetUserData', "").subscribe(result => {
        //this.forecasts = result;
        console.log(result);
      }, error => console.error(error));

      http.post<User>(baseUrl + 'Users/authenticate', req).subscribe(result => {
        if (result.token !== null) {
          //localStorage.setItem('jwttoken', result.token);
        }
        console.log(result);
      });

    }, error => console.error(error));

  

  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

interface User {
  id: number;
  firstName: string;
  lastName: string;
  username: string;
  password: string;
  role: string;
  token: string;
}
