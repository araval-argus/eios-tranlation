import { AuthGuard } from './core/store/auth/auth.guard';

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', 
    loadChildren: () => import('./features/index/index.module').then( m => m.IndexModule)
  },
  // {
  //   path: 'login', 
  //   loadChildren: () => import('./features/auth/auth.module').then( m => m.AuthModule)
  // },
  { path: '', redirectTo: '/', pathMatch: 'full' },
  { 
    path: '**', 
    redirectTo: '/'
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
