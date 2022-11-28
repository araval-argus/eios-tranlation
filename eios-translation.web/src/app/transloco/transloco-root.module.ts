import { HttpClient } from '@angular/common/http';
import {
  TRANSLOCO_CONFIG, TRANSLOCO_LOADER, Translation, TranslocoLoader, translocoConfig, TranslocoModule
} from '@ngneat/transloco';
import { Injectable, NgModule } from '@angular/core';
import { environment } from '../../environments/environment';
import { defaultTranslocoMarkupTranspilers } from 'ngx-transloco-markup';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TranslocoHttpLoader implements TranslocoLoader {
  constructor(private http: HttpClient) {}

  public getTranslation(language: string): Observable<Translation> {
    return this.http.get<Translation>(`/assets/i18n/${language}.json`);
  }
}

@NgModule({
  exports: [ 
    TranslocoModule
  ],
  providers: [
    {
      provide: TRANSLOCO_CONFIG,
      useValue: translocoConfig({
        availableLangs: ['en', 'it'],
        defaultLang: 'en',
        // Remove this option if your application doesn't support changing language in runtime.
        reRenderOnLangChange: true,
        prodMode: environment.production,
      })
    },
    {
      provide: TRANSLOCO_LOADER,
      useClass: TranslocoHttpLoader,
    },
    defaultTranslocoMarkupTranspilers()
  ]
})
export class TranslocoRootModule {}
