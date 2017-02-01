import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { Exposure } from './exposure';
import { Position } from './position';

@Injectable()
export class ExposureService {
  private baseUrl: string = 'http://localhost:3374/api';

  constructor(private http: Http) {
  }

  getAll(): Observable<Exposure[]> {
    let exposures$ = this.http
      .get(`${this.baseUrl}/ViewPosition`, { headers: this.getHeaders() })
      .map(mapExposures)
      .catch(handleError);
    return exposures$;
  }

  get(id: number): Observable<Exposure> {
    let exposure$ = this.http
      .get(`${this.baseUrl}/exposure/${id}`, { headers: this.getHeaders() })
      .map(mapExposure);
    return exposure$;
  }

  save(exposure: Exposure): Observable<Response> {
    return this.http
      .put(`${this.baseUrl}/exposure/${exposure.Key}`, JSON.stringify(exposure), { headers: this.getHeaders() });
  }

  private getHeaders() {
    let headers = new Headers();
    headers.append('Accept', 'application/json');
    return headers;
  }
}

function mapExposures(response: Response): Exposure[] {
  // uncomment to simulate error:
  // throw new Error('ups! Force choke!');

  // The response of the API has a results
  // property with the actual results
  return response.json().map(toExposure)
}

function toExposure(r: any): Exposure {
  let exposure = <Exposure>r;
  console.log('Parsed exposure:', exposure);
  return exposure;
}

function mapExposure(response: Response): Exposure {
  return toExposure(response.json());
}

// this could also be a private method of the component class
function handleError(error: any) {
  // log error
  // could be something more sofisticated
  let errorMsg = error.message || `Yikes! We couldnt retrieve your data!`
  console.error(errorMsg);

  // throw an application level error
  return Observable.throw(errorMsg);
}