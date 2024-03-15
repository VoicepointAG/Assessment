import { Component } from '@angular/core';
import { HangmanClient } from './api/web-api.module';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'voicepoint-assessment';
  name = '';
  greeting = '';
  imageSource = 'assets/hangman-7.svg';

  constructor(private readonly _hangmanClient: HangmanClient) {

  }

  async onNameChange() {
    const response=await this._hangmanClient.sayHello(this.name).toPromise();
    this.greeting=response?.hello ?? '';
  }
}
