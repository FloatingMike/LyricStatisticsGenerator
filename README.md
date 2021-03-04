<!-- PROJECT SHIELDS -->

[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<p align="center">
  <h3 align="center">Artist Lyric Statistics</h3>

  <p align="center">
    Getting you the lyric statistics you really need... eventually.
    <br />
    <a href="https://github.com/MikeVerius/Aire"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    ·
    <a href="https://github.com/MikeVerius/Aire/issues">Report Bug</a>
    ·
    <a href="https://github.com/MikeVerius/Aire/issues">Request Feature</a>
  </p>
</p>



<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary><h2 style="display: inline-block">Table of Contents</h2></summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

[![Product Name Screen Shot][product-screenshot]]()

### Built With

* [Microsoft .NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)
* [xUnit](https://xunit.net)
* [MusicBrainz](https://musicbrainz.org)
* [LyricsOvh](https://lyrics.ovh)


<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites

* dotnet core 
  Please download the appropriate Runtime package for your platform from [here](https://dotnet.microsoft.com/download/dotnet/3.1)
  
### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/MikeVerius/Aire.git
   ```
2. Run the tests
   ```sh
   dotnet test
   ```
3. 
   ```sh
   dotnet run --project AireLogicTest.Console
   ```
   (Alternatively you can switch into the AireLogicTest.Console directory and just use `dotnet run` from there)




<!-- USAGE EXAMPLES -->
## Usage

* An artist name can be entered as an argument to avoid an initial prompt
  ```sh
  dotnet run "Depeche Mode"
  ```
  You will then be prompted to select an artist name from a list of potential matches, the most likely match will be the first item with the identifier "0" however if you do not find the result you want, enter no value and you will be prompted to input a new artist name to try again.

  If the artist has previously been searched successfully the result will be calculated from locally cached versions of the lyrics otherwise depending on the artist selected it may take many minutes to generate the output.


<!-- ROADMAP -->
## Roadmap

See the [open issues](https://github.com/MikeVerius/Aire/issues) for a list of proposed features (and known issues).



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request


<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.


<!-- CONTACT -->
## Contact

Michael Hardman - [@MikeDoesCode](https://twitter.com/MikeDoesCode) - Mike.Hardman.Work@gmail.com

Project Link: [https://github.com/MikeVerius/Aire](https://github.com/MikeVerius/Aire)


[issues-shield]: https://img.shields.io/github/issues/MikeVerius/repo.svg?style=for-the-badge
[issues-url]: https://github.com/MikeVerius/repo/issues
[license-shield]: https://img.shields.io/github/license/MikeVerius/repo.svg?style=for-the-badge
[license-url]: https://github.com/MikeVerius/repo/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/MikeVerius
