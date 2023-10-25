import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent {
  baseUrl = 'https://localhost:5001/api/';
  validationerror:string[] = [];

  constructor(
    private http:HttpClient
  ){}



  get404Error(){
    this.http.get(this.baseUrl + 'Buggy/not-found').subscribe({
      next: response => console.log(response),
      error: errro => console.log(errro)
    })
  }

  get400Error(){
    this.http.get(this.baseUrl + 'Buggy/bad-request').subscribe({
      next: response => console.log(response),
      error: errro => console.log(errro)
    })
  }

  get500Error(){
    this.http.get(this.baseUrl + 'Buggy/server-error').subscribe({
      next: response => console.log(response),
      error: errro => console.log(errro)
    })
  }


  get401Error(){
    this.http.get(this.baseUrl + 'Buggy/auth').subscribe({
      next: response => console.log(response),
      error: errro => console.log(errro)
    })
  }

  
  get400ValidationError(){
    this.http.post(this.baseUrl + 'account/register', {}).subscribe({
      next: response => console.log(response),
      error: error =>{ 
        console.log(error)
        this.validationerror = error;
      }
    });
  }


}
