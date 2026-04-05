import { Routes } from '@angular/router';
import { BoardComponent } from './board/board.component';
import { LoginComponent } from './components/login/login'; 

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'board', component: BoardComponent }
];